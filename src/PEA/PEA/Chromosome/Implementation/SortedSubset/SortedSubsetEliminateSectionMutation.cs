using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetEliminateSectionMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetEliminateSectionMutation(IRandom random, ParameterSet parameterSet) : base(random,parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
