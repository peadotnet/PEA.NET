using Pea.Core;
using System;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class RelocateOneMutation : PermutationMutationBase
    {
        public RelocateOneMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public override PermutationChromosome Mutate(PermutationChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Genes.Length < 2) return chromosome;

            //TODO: ConflictCheck, retry

            //int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            //while (true)
            //{
            var sourcePosition = Random.GetInt(0, chromosome.Genes.Length);

            int targetPosition = Random.GetIntWithTabu(0, chromosome.Genes.Length, sourcePosition);

            if (sourcePosition > targetPosition)
            {
                var tempPos = sourcePosition;
                sourcePosition = targetPosition;
                targetPosition = tempPos;
            }

            var sourceRange = new GeneRange(sourcePosition, 1);
            var swapRange = new GeneRange(sourcePosition+1, targetPosition - sourcePosition-1);
            chromosome.Genes = SwapTwoRange(chromosome.Genes, sourceRange, swapRange);
            
            //    if (success || retryCount-- < 0) break;
            //}
            return chromosome;
        }
    }
}
