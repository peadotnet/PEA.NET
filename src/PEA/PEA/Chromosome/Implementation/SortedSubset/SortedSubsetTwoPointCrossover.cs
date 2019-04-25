using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetTwoPointCrossover : SortedSubsetOperatorBase, ICrossover<SortedSubsetChromosome>
    {
        public SortedSubsetTwoPointCrossover(IRandom random, ParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents)
        {
            throw new System.NotImplementedException();
        }
    }
}
