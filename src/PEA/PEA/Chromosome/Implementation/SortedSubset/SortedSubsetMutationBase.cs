using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public abstract class SortedSubsetMutationBase : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        protected SortedSubsetMutationBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome);

        public virtual IChromosome Mutate(IChromosome chromosome)
        {
            var sortedSubsetChromosome = chromosome as SortedSubsetChromosome;
            if (sortedSubsetChromosome == null) throw new ArgumentException(nameof(chromosome));

            return Mutate(sortedSubsetChromosome);
        }
    }
}
