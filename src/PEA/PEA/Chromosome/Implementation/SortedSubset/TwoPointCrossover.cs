using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class TwoPointCrossover : SortedSubsetOperatorBase, ICrossover<SortedSubsetChromosome>
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents)
        {
            throw new System.NotImplementedException();
        }
    }
}
