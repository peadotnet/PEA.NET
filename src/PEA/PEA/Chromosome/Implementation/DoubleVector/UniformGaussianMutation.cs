﻿using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class UniformGaussianMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public UniformGaussianMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var original = chromosome as DoubleVectorChromosome;
            var length = original.Genes.Length;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            var mutated = new double[length];
            var modification = Random.GetGaussian(0, mutationIntensity);

            for (int i = 0; i < length; i++)
            {
                var gene = original.Genes[i];

                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    gene += modification;
                }

                mutated[i] = gene;
            }

            var mutatedChromosome = new DoubleVectorChromosome(mutated);
            return mutatedChromosome;
        }
    }
}