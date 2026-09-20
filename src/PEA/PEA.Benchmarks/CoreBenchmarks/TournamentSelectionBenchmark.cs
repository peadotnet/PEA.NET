using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Pea.Core;
using Pea.Core.Entity;
using Pea.Fitness.Implementation.MultiObjective;

namespace PEA.Benchmarks.CoreBenchmarks
{
    /// <summary>
    /// The same indexer comparison as PopulationIndexerBenchmark, but in context:
    /// inside a faithful copy of TournamentSelection.SelectOne, with the real
    /// Pareto fitness comparer doing the work alongside it.
    ///
    /// PopulationIndexerBenchmark answers "what does the indexer write cost".
    /// This one answers "does it matter once the fitness comparison is in the picture" —
    /// which is the number that should decide whether the change is worth making
    /// for performance reasons (as opposed to correctness reasons, of which there
    /// are several independent ones).
    ///
    /// Random index generation is hoisted into setup so the measurement covers the
    /// selection loop only, not the RNG.
    /// </summary>
    [MemoryDiagnoser]
    [JsonExporterAttribute.Full]
    [MinColumn, MaxColumn]
    public class TournamentSelectionBenchmark
    {
        [Params(100, 1000, 10000)]
        public int PopulationSize { get; set; }

        /// <summary>Matches SteadyState.GetParameters()'s default TournamentSize.</summary>
        [Params(4)]
        public int TournamentSize { get; set; }

        private const int SelectionsPerInvocation = 256;

        private IndexerWithWrite _withWrite;
        private IndexerPure _pure;
        private IFitnessComparer _comparer;
        private int[] _indices;

        [GlobalSetup]
        public void Setup()
        {
            var random = new System.Random(20260919);

            var entities = new List<EntityBase>(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                var entity = new BenchEntity();
                // Two objectives, so the comparer exercises real Pareto dominance logic
                // (including the incomparable case) rather than a degenerate single-value path.
                entity.SetFitness(new MultiObjectiveFitness(new[] { random.NextDouble(), random.NextDouble() }));
                entities.Add(entity);
            }

            _withWrite = new IndexerWithWrite(entities);
            _pure = new IndexerPure(entities);
            _comparer = new ParetoComparerWithConstraintViolationReduction();

            _indices = new int[SelectionsPerInvocation * TournamentSize];
            for (int i = 0; i < _indices.Length; i++)
            {
                _indices[i] = random.Next(0, PopulationSize);
            }
        }

        [Benchmark(Baseline = true)]
        public EntityBase SelectWithWritingIndexer()
        {
            EntityBase last = null;
            int cursor = 0;
            for (int s = 0; s < SelectionsPerInvocation; s++)
            {
                var best = _withWrite[_indices[cursor++]];
                for (int i = 1; i < TournamentSize; i++)
                {
                    var next = _withWrite[_indices[cursor++]];
                    best = Compete(best, next);
                }
                last = best;
            }
            return last;
        }

        [Benchmark]
        public EntityBase SelectWithPureIndexer()
        {
            EntityBase last = null;
            int cursor = 0;
            for (int s = 0; s < SelectionsPerInvocation; s++)
            {
                var best = _pure[_indices[cursor++]];
                for (int i = 1; i < TournamentSize; i++)
                {
                    var next = _pure[_indices[cursor++]];
                    best = Compete(best, next);
                }
                last = best;
            }
            return last;
        }

        /// <summary>
        /// Mirrors the body of TournamentSelection.SelectOne, including the winner/loser
        /// counter updates — those are themselves writes into scattered fitness objects
        /// on every comparison, so leaving them out would understate the real loop.
        /// </summary>
        private EntityBase Compete(EntityBase best, EntityBase next)
        {
            var comparisonResult = _comparer.Compare(best.Fitness, next.Fitness);
            if (comparisonResult > 0)
            {
                next.Fitness.TournamentWinner++;
                best.Fitness.TournamentLoser++;
                return next;
            }

            best.Fitness.TournamentWinner++;
            next.Fitness.TournamentLoser++;
            return best;
        }

        private class BenchEntity : EntityBase
        {
            public int IndexInPopulation;

            public BenchEntity() : base(1)
            {
            }
        }

        private class IndexerWithWrite
        {
            private readonly IList<EntityBase> _entities;
            public IndexerWithWrite(IList<EntityBase> entities) => _entities = entities;

            public EntityBase this[int index]
            {
                get
                {
                    ((BenchEntity)_entities[index]).IndexInPopulation = index;
                    return _entities[index];
                }
            }
        }

        private class IndexerPure
        {
            private readonly List<EntityBase> _entities;
            public IndexerPure(IList<EntityBase> entities) => _entities = (List<EntityBase>)entities;

            public EntityBase this[int index] => _entities[index];
        }
    }
}
