using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public abstract class BitVectorMutationBase : BitVectorOperationBase, IMutation<BitVectorChromosome>
	{
		protected BitVectorMutationBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
		{
		}

		public abstract BitVectorChromosome Mutate(BitVectorChromosome chromosome);

		public IChromosome Mutate(IChromosome chromosome)
		{
			var bitVectorChromosome = chromosome as BitVectorChromosome;
			if (bitVectorChromosome == null) throw new ArgumentException(nameof(chromosome));

			return Mutate(bitVectorChromosome);
		}
	}
}
