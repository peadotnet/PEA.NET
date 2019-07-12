using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SwapTwoRangeMutation : SortedSubsetMutationBase
    {
        public SwapTwoRangeMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome entity)
        {
            throw  new NotImplementedException();
        }
    }
}
