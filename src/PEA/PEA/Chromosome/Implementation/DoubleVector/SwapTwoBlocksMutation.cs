﻿using Pea.Core;
using System.Collections.Generic;


namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class SwapTwoBlocksMutation : DoubleVectorOperatorBase, IMutation<DoubleVectorChromosome>
    {
        public SwapTwoBlocksMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IChromosome Mutate(IChromosome chromosome)
        {
            var genes = chromosome as DoubleVectorChromosome;
            var length = genes.Genes.Length;

            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);
            if (length / blockSize < 3) return genes;

            var blockPosition1 = Random.GetInt(1, length / blockSize);
            var blockPosition2 = Random.GetIntWithTabu(1, length / blockSize, blockPosition1);

            var position1 = blockPosition1 * blockSize;
            var position2 = blockPosition2 * blockSize;

            for (int i=0; i< blockSize; i++)
			{
                var value1 = genes.Genes[position1 + i];
                var value2 = genes.Genes[position2 + i];

                genes.Genes[position1 + i] = value2;
                genes.Genes[position2 + i] = value1;

            }

            return chromosome;
        }
    }
}