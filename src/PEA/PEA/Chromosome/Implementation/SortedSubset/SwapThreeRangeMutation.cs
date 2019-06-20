using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SwapThreeRangeMutation : SortedSubsetMutationBase
    {
        public SwapThreeRangeMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
