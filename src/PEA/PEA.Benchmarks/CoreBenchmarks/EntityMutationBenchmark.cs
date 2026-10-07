using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Core.Entity;

namespace PEA.Benchmarks.CoreBenchmarks
{
    [MemoryDiagnoser]
    [JsonExporterAttribute.Full]
    [MinColumn, MaxColumn]
    public class EntityMutationBenchmark
    {
        [Params(52, 104)]
        public int GeneCount { get; set; }

        [Params(2, 100)]
        public int OffspringCount { get; set; }

        private const string GeneKey = "permutation";

        private IEntityMutation _sut;
        private IEntityList _offspring;

        [GlobalSetup]
        public void Setup()
        {
            var random = new FastRandom(20260929);
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector> { AllRightConflictDetector.Instance };
            var mutation = new RelocateRangeMutation(random, parameterSet, conflictDetectors);

            var factories = new Dictionary<string, IChromosomeFactory>
            {
                { GeneKey, new SingleMutationFactory(mutation) }
            };
            _sut = new EntityMutation(factories, random);

            var creator = new PermutationRandomCreator(GeneCount, random, conflictDetectors);
            _offspring = new EntityList(OffspringCount);
            for (int i = 0; i < OffspringCount; i++)
            {
                var entity = new BenchEntity();
                entity.Chromosomes.Add(GeneKey, creator.Create());
                _offspring.Add(entity);
            }
        }

        [Benchmark]
        public IEntityList Mutate()
        {
            _sut.Mutate(_offspring);
            return _offspring;
        }

        public sealed class BenchEntity : EntityBase
        {
            public BenchEntity() : base(1)
            {
            }
        }

        private sealed class SingleMutationFactory : IChromosomeFactory
        {
            private readonly IMutation _mutation;

            public SingleMutationFactory(IMutation mutation) => _mutation = mutation;

            public IList<IMutation> GetMutations() => new List<IMutation> { _mutation };

            public IEnumerable<PeaSettingsNamedValue> GetParameters() => new List<PeaSettingsNamedValue>();

            public IList<IChromosomeCreator> GetCreators() => new List<IChromosomeCreator>();

            public IList<ICrossover> GetCrossovers() => new List<ICrossover>();

            public IEngine Apply(IEngine engine) => engine;
        }
    }
}
