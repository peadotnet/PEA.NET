using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class UniformBlockParallelMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public UniformBlockParallelMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var original = chromosome as DoubleVectorChromosome;
            var length = original.Genes.Length;

            //int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            var mutated = new double[length];

            int relPos = Random.GetInt(0, blockSize);
            double modification = Random.GetGaussian(0, mutationIntensity);

            for (int block = 0; block < length / blockSize; block++)
            {
                var position = blockSize * block + relPos;
                var gene = original.Genes[position];

                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    gene += modification;
                }
                mutated[position] = gene;
            }

            var mutatedChromosome = new DoubleVectorChromosome(mutated);
            return mutatedChromosome;

        }
    }
}
