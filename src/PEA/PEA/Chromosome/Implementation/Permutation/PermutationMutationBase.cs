﻿using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public abstract class PermutationMutationBase : PermutationOperatorBase, IMutation<PermutationChromosome>
    {
        protected PermutationMutationBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract PermutationChromosome Mutate(PermutationChromosome chromosome);

        public IChromosome Mutate(IChromosome chromosome)
        {
            var sortedSubsetChromosome = chromosome as PermutationChromosome;
            if (sortedSubsetChromosome == null) throw new ArgumentException(nameof(chromosome));

            return Mutate(sortedSubsetChromosome);
        }
    }
}
