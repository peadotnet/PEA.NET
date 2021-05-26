using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class OnePointGaussianMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public OnePointGaussianMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);

            var position = Random.GetInt(0, length);
            var oldValue = genes.Genes[position];
            var newValue = Random.GetGaussian(oldValue, mutationIntensity);

            genes.Genes[position] = newValue;

            return chromosome;
        }
    }
}
