using Pea.Core;
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
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);

            for (int i = 0; i < length; i++)
            {
                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    var gene = genes.Genes[i];
                    gene = Random.GetGaussian(gene, mutationIntensity);
                    genes.Genes[i] = gene;
                }

            }

            return genes;
        }
    }
}
