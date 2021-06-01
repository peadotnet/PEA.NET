using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class OneBlockRegenerationMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public OneBlockRegenerationMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);
            int blocksCount = length / blockSize;

            var block = Random.GetInt(0, blocksCount);

            for (int pos = 0; pos < blockSize; pos++)
            {
                double minValue = double.MaxValue;
                double maxValue = double.MinValue;

                for(int b = 0; b < blocksCount; b++)
				{
                    var gene = genes.Genes[b * blockSize + pos];
                    if (gene < minValue) minValue = gene;
                    if (gene > maxValue) maxValue = gene;
				}

                var newValue = Random.GetDouble(minValue, maxValue);
                genes.Genes[block * blockSize + pos] = newValue;
            }
            return chromosome;
        }
    }
}
