using BenchmarkDotNet.Attributes;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using System.Collections.Generic;

namespace PEA.Benchmarks.ChromosomeBenchmarks
{
    public class PermutationOperatorBenchmarks
    {
        public const int Count = 100;
        public const int Size = 52;

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
            List<IChromosome> result = new List<IChromosome>();
            for (int i=0; i< Chromosomes.Count; i++)
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
