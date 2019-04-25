using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetReplaceRangeMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetReplaceRangeMutation(IRandom random, ParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
