using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class UniformParallelMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public UniformParallelMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
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

            var mutated = new double[length];

            for (int i = 0; i < length; i++)
            {
                var gene = original.Genes[i];

                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    var newValue = Random.GetGaussian(gene, mutationIntensity);
                    if (newValue < 0) newValue = 0;
                    if (newValue > 1) newValue = 1 - double.Epsilon;
                    gene = newValue;
                }
            }

            var mutatedChromosome = new DoubleVectorChromosome(mutated);
            return mutatedChromosome;
        }
    }
}
