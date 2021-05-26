using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    class AllBlockParallelMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public AllBlockParallelMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            var mutationIntensity = ParameterSet.GetValue(ParameterNames.MutationIntensity);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            int relPos = Random.GetInt(0, blockSize);
            double modification = Random.GetGaussian(0, mutationIntensity);

            for (int block = 0; block < length / blockSize; block++)
            {
                for (int position = 0; position < blockSize; position++)
                {
                    var gene = genes.Genes[blockSize * block + position];
                    if (position == relPos) gene += modification;
                    genes.Genes[blockSize * block + position] = gene;
                }
            }

            return genes;
        }
    }
}