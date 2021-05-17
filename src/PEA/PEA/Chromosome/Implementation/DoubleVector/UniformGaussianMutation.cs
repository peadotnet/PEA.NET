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

            var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);

            var mutated = new double[length];

            for (int i = 0; i < length; i++)
            {
                var gene = original.Genes[i];

                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    gene = Random.GetGaussian(gene, mutationIntensity);
                }

                mutated[i] = gene;
            }

            var mutatedChromosome = new DoubleVectorChromosome(mutated);
            return mutatedChromosome;
        }
    }
}
