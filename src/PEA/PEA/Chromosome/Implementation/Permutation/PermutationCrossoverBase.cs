using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public abstract class PermutationCrossoverBase : PermutationOperatorBase, ICrossover<PermutationChromosome>
    {
        protected PermutationCrossoverBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
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
