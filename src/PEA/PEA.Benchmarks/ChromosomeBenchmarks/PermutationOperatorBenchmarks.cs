using BenchmarkDotNet.Attributes;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using System.Collections.Generic;
using System.Linq;

namespace PEA.Benchmarks.ChromosomeBenchmarks
{
    [MinColumn, MaxColumn]
    [HtmlExporter, RPlotExporter]
    public class PermutationOperatorBenchmarks
    {
        [Params(100)]
        public int Count { get; set; }

        [Params(52, 104)]
        public int Size { get; set; }

        public IRandom Random = new FastRandom();

        List<IChromosome> Chromosomes = new List<IChromosome>();

        public ParameterSet ParameterSet = new ParameterSet();

        public PermutationOperatorBenchmarks()
        {
            var conflictDetector = AllRightConflictDetector.Instance;
            var chromosomeCreator = new PermutationRandomCreator(Size, Random, conflictDetector);
            for (int i = 0; i < Count; i++)
            {
                Chromosomes.Add(chromosomeCreator.Create());
            }
        }

        public List<IChromosome> MutateChromosomes(IMutation mutation)
        {
            var result = new List<IChromosome>();
            for (int i = 0; i < Chromosomes.Count; i++)
            {
                result.Add(mutation.Mutate(Chromosomes[i]));
            }
            return result;
        }

        [Benchmark]
        public List<IChromosome> RelocateRange()
        {
            var mutation = new RelocateRangeMutation(Random, ParameterSet, AllRightConflictDetector.Instance);
            return MutateChromosomes(mutation);
        }

        [Benchmark]
        public List<IChromosome> InverseRange()
        {
            var mutation = new InverseRangeMutation(Random, ParameterSet, AllRightConflictDetector.Instance);
            return MutateChromosomes(mutation);
        }

        [Benchmark]
        public List<IChromosome> SwapTwoRange()
        {
            var mutation = new SwapTwoRangeMutation(Random, ParameterSet, AllRightConflictDetector.Instance);
            return MutateChromosomes(mutation);
        }
    }
}
