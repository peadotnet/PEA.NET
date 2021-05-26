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
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            var mutationProbability = ParameterSet.GetValue(ParameterNames.MutationProbability);
            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            int relPos = Random.GetInt(0, blockSize);
            double modification = Random.GetGaussian(0, mutationIntensity);

            for (int block = 0; block < length / blockSize; block++)
            {
                var rnd = Random.GetDouble(0, 1);
                if (rnd < mutationProbability)
                {
                    var position = blockSize * block + relPos;
                    var gene = genes.Genes[position];
                    gene += modification;
                    genes.Genes[position] = gene;
                }
            }

            return genes;
        }
    }
}
