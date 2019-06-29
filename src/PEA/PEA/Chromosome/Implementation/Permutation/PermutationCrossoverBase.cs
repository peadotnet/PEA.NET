using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public abstract class PermutationCrossoverBase : PermutationOperatorBase, ICrossover<PermutationChromosome>
    {
        protected PermutationCrossoverBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract IList<PermutationChromosome> Cross(IList<PermutationChromosome> parents);

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var sortedSubsetParents = parents as IList<PermutationChromosome>;
            if (sortedSubsetParents == null) throw new ArgumentException(nameof(parents));

            return Cross(sortedSubsetParents) as IList<IChromosome>;
        }
    }
}
