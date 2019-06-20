using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class EliminateSectionMutation : SortedSubsetMutationBase
    {
        public EliminateSectionMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
