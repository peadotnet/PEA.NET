using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class TwoPointCrossover : SortedSubsetCrossoverBase
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents)
        {
            throw new System.NotImplementedException();
        }
    }
}
