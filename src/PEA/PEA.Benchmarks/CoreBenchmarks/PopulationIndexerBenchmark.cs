using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Pea.Core;
using Pea.Core.Entity;
using Pea.Population;

namespace PEA.Benchmarks.CoreBenchmarks
{
    /// <summary>
    /// Measures the cost of a population/entity-list indexer read.
    ///
    /// The baseline is a structural reproduction of the indexer as it stood before the
    /// index-maintenance rework (git af3c011), where the getter also wrote the index back:
    ///     private IList&lt;IPopulationEntity&gt; Entities { get; set; }
    ///     get { Entities[index].IndexInPopulation = index; return Entities[index]; }
    /// The element type was an interface, so the write was a property call, and the list was
    /// read twice. It is reproduced here rather than compared across runs so that the old and
    /// the new shape are measured in the same process, on the same JIT, over the same indices.
    ///
    /// The Real* arms read the shipping FlatPopulation and EntityList through their public
    /// indexers; they are the regression guard. ListPureRead and ArrayPureRead mark what is
    /// still left on the table below the current implementation.
    /// </summary>
    [MemoryDiagnoser]
    [JsonExporterAttribute.Full]
    [MinColumn, MaxColumn]
    public class PopulationIndexerBenchmark
    {
        /// <summary>Random access over a small population stays in cache; a large one does not.</summary>
        [Params(100, 1000, 10000)]
        public int PopulationSize { get; set; }

        /// <summary>Accesses performed per benchmark invocation — reported times are for this many reads.</summary>
        private const int AccessCount = 1024;

        private LegacyPopulation _legacyPopulation;
        private FlatPopulation _realPopulation;
        private EntityList _realEntityList;
        private ListPure _listPure;
        private ArrayPure _arrayPure;

        /// <summary>Pre-generated so random number generation is not part of the measurement.</summary>
        private int[] _indices;

        [GlobalSetup]
        public void Setup()
        {
            var entities = new List<EntityBase>(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                entities.Add(new EntityBase(1));
            }

            _realPopulation = new FlatPopulation(1, PopulationSize, PopulationSize);
            _realEntityList = new EntityList(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                _realPopulation.Add(entities[i]);
                _realEntityList.Add(entities[i]);
            }

            _listPure = new ListPure(entities);
            _arrayPure = new ArrayPure(entities);

            var legacyEntities = new List<ILegacyEntity>(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                legacyEntities.Add(new LegacyEntity());
            }
            _legacyPopulation = new LegacyPopulation(legacyEntities);

            // Deterministic pseudo-random walk: fixed seed so every arm and every run sees
            // exactly the same access pattern.
            var random = new System.Random(20260919);
            _indices = new int[AccessCount];
            for (int i = 0; i < AccessCount; i++)
            {
                _indices[i] = random.Next(0, PopulationSize);
            }
        }

        /// <summary>The indexer as it stood before the rework — the "before" reference.</summary>
        [Benchmark(Baseline = true)]
        public ILegacyEntity LegacyWritingIndexer()
        {
            ILegacyEntity last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _legacyPopulation[_indices[i]];
            }
            return last;
        }

        /// <summary>The shipping FlatPopulation indexer.</summary>
        [Benchmark]
        public EntityBase RealFlatPopulationRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _realPopulation[_indices[i]];
            }
            return last;
        }

        /// <summary>The shipping EntityList indexer.</summary>
        [Benchmark]
        public EntityBase RealEntityListRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _realEntityList[_indices[i]];
            }
            return last;
        }

        /// <summary>What the shipping types would cost if the backing field were the concrete List.</summary>
        [Benchmark]
        public EntityBase ListPureRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _listPure[_indices[i]];
            }
            return last;
        }

        /// <summary>Lower bound: raw array access.</summary>
        [Benchmark]
        public EntityBase ArrayPureRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _arrayPure[_indices[i]];
            }
            return last;
        }

        /// <summary>
        /// The index fields on EntityBase are internal, so the "before" shape is reproduced with
        /// its own interface rather than by referencing the removed IPopulationEntity.
        /// </summary>
        public interface ILegacyEntity
        {
            int IndexInPopulation { get; set; }
        }

        private class LegacyEntity : EntityBase, ILegacyEntity
        {
            public int IndexInPopulation { get; set; }

            public LegacyEntity() : base(1)
            {
            }
        }

        private class LegacyPopulation
        {
            private readonly IList<ILegacyEntity> _entities;
            public LegacyPopulation(IList<ILegacyEntity> entities) => _entities = entities;

            public ILegacyEntity this[int index]
            {
                get
                {
                    _entities[index].IndexInPopulation = index;
                    return _entities[index];
                }
            }
        }

        private class ListPure
        {
            private readonly List<EntityBase> _entities;
            public ListPure(List<EntityBase> entities) => _entities = entities;

            public EntityBase this[int index] => _entities[index];
        }

        private class ArrayPure
        {
            private readonly EntityBase[] _entities;
            public ArrayPure(List<EntityBase> entities) => _entities = entities.ToArray();

            public EntityBase this[int index] => _entities[index];
        }
    }
}
