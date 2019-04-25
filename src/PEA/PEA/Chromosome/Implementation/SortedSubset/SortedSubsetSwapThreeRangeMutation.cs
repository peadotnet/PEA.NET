using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetSwapThreeRangeMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetSwapThreeRangeMutation(IRandom random, ParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
