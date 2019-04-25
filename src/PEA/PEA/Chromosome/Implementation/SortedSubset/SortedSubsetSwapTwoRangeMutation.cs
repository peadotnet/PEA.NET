using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetSwapTwoRangeMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetSwapTwoRangeMutation(IRandom random, ParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
