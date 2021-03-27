using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public abstract class PermutationMutationBase : PermutationOperatorBase, IMutation<PermutationChromosome>
    {
        protected PermutationMutationBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
        {
        }

        public abstract PermutationChromosome Mutate(PermutationChromosome chromosome);

        public IChromosome Mutate(IChromosome chromosome)
        {
            var permutationChromosome = chromosome as PermutationChromosome;
            if (permutationChromosome == null) throw new ArgumentException(nameof(chromosome));

            return Mutate(permutationChromosome);
        }
    }
}
