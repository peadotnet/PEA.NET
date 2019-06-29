using System;
using System.Collections.Generic;
using System.Text;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PMXCrossover : PermutationCrossoverBase
    {
        public PMXCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public override IList<PermutationChromosome> Cross(IList<PermutationChromosome> parents)
        {
            throw new NotImplementedException();
        }
    }
}
