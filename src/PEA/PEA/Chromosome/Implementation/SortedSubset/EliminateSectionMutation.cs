using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class EliminateSectionMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public EliminateSectionMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
