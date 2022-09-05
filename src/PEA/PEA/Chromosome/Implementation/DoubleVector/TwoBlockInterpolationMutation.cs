using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class TwoBlockInterpolationMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public TwoBlockInterpolationMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);
            int blocksCount = length / blockSize;

            var block1 = Random.GetInt(0, blocksCount);
            var block2 = Random.GetIntWithTabu(0, blocksCount, block1);
            double weight = Random.GetDouble(-1, 1);

            for (int pos = 0; pos < blockSize; pos++)
            {
                var newValue = weight * genes.Genes[block1 * blockSize + pos] + (1 - weight) * genes.Genes[block2 * blockSize + pos];
                genes.Genes[block1 * blockSize + pos] = newValue;
            }
            return chromosome;
        }
    }
}
