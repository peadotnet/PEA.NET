using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class SwapTwoRangeMutation : PermutationMutationBase
    {
        public SwapTwoRangeMutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
        {
        }

        public override PermutationChromosome Mutate(PermutationChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Genes.Length < 2) return null;

            //TODO: conflictDetection, repeat, conflictList cleaning

            bool success = false;
            while (!success)
            {
                var range1 = GetSourceRange(chromosome);
                var range2 = GetSourceRange(chromosome);
                success = range1.IsDisjointWith(range2);
                if (success)
                {
                    chromosome.Genes = SwapTwoRange(chromosome.Genes, range1, range2);
                }
            }
            return chromosome;
        }
    }
}
