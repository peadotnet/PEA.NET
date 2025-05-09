using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
    public class OneBlockMutation : BitVectorMutationBase
    {
        public OneBlockMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override BitVectorChromosome Mutate(BitVectorChromosome chromosome)
        {
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            var block = Random.GetInt(0, chromosome.Genes.Length / blockSize);
            int pos = blockSize * block;

            for (int i = 0; i < blockSize; i++)
            {
                chromosome.Genes[pos + i] = !chromosome.Genes[pos + i];
            }
            return chromosome;
        }
    }
}
