using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class OneBlockGaussianMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public OneBlockGaussianMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            //var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            var block = Random.GetInt(0, length / blockSize);
            var beginPosition = block * blockSize;

            for (int position = block * blockSize; position < (block + 1) * blockSize - 1; position++)
            {
                var oldValue = genes.Genes[position];
                var newValue = Random.GetGaussian(oldValue, mutationIntensity);

                //         bool success = false;
                //         while (!success && retryCount > 0)
                //         {
                //             bool conflicted = false;
                //             for (int i = 0; i < ConflictDetectors.Count; i++)
                //             {
                //                 var detector = ConflictDetectors[i] as IPositionValueConflictDetector<double>;
                //                 if (!detector.ConflictDetected(chromosome.Entity, position, newValue)) continue;
                //                 conflicted = true;
                //                 break;
                //             }

                //             success = !conflicted;
                //             retryCount--;
                //         }

                //         if (success)
                //{
                genes.Genes[position] = newValue;
                //}
            }
            return chromosome;
        }
    }
}
