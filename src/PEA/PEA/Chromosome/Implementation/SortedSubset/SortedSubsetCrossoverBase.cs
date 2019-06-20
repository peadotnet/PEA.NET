using System;
using System.Collections.Generic;
using System.Text;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public abstract class SortedSubsetCrossoverBase : SortedSubsetOperatorBase, ICrossover<SortedSubsetChromosome>
    {
        protected SortedSubsetCrossoverBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents);

        public virtual IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var sortedSubsetParents = parents as IList<SortedSubsetChromosome>;
            if (sortedSubsetParents == null) throw new ArgumentException(nameof(parents));

            return Cross(sortedSubsetParents) as IList<IChromosome>;
        }
    }
}
