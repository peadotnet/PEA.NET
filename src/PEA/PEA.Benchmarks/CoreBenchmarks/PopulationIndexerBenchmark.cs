using BenchmarkDotNet.Attributes;
using Pea.Core;
using Pea.Core.Entity;
using Pea.Population;

namespace PEA.Benchmarks.CoreBenchmarks
{
    [MemoryDiagnoser]
    [JsonExporterAttribute.Full]
    [MinColumn, MaxColumn]
    public class PopulationIndexerBenchmark
    {
        [Params(100, 1000, 10000)]
        public int PopulationSize { get; set; }

        private const int AccessCount = 1024;

        private FlatPopulation _population;
        private EntityList _entityList;
        private int[] _indices;

        [GlobalSetup]
        public void Setup()
        {
            _population = new FlatPopulation(1, PopulationSize, PopulationSize);
            _entityList = new EntityList(PopulationSize);
            for (int i = 0; i < PopulationSize; i++)
            {
                var entity = new EntityBase(1);
                _population.Add(entity);
                _entityList.Add(entity);
            }

            var random = new System.Random(20260919);
            _indices = new int[AccessCount];
            for (int i = 0; i < AccessCount; i++)
            {
                _indices[i] = random.Next(0, PopulationSize);
            }
        }

        [Benchmark]
        public EntityBase FlatPopulationRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _population[_indices[i]];
            }
            return last;
        }

        [Benchmark]
        public EntityBase EntityListRead()
        {
            EntityBase last = null;
            for (int i = 0; i < _indices.Length; i++)
            {
                last = _entityList[_indices[i]];
            }
            return last;
        }
    }
}
