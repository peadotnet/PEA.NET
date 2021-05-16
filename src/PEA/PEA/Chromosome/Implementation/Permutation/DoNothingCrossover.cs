using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class DoNothingCrossover : PermutationCrossoverBase
    {
        public DoNothingCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override IList<IChromosome> Cross(IChromosome iparent0, IChromosome iparent1)
        {
            var offspring = new List<IChromosome>();
            offspring.Add(iparent0.DeepClone());
            offspring.Add(iparent1.DeepClone());
            return offspring;
        }
    }
}
