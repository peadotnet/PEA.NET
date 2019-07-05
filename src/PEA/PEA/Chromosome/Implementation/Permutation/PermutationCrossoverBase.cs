using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public abstract class PermutationCrossoverBase : PermutationOperatorBase, ICrossover<PermutationChromosome>
    {
        protected PermutationCrossoverBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract IList<IChromosome> Cross(IList<IChromosome> parents);

        //public IList<IChromosome> Cross(IList<IChromosome> parents)
        //{
        //    var sortedSubsetParents = parents.Cast<PermutationChromosome>().ToList();

        //    return Cross(sortedSubsetParents) as IList<IChromosome>;
        //}
    }
}
