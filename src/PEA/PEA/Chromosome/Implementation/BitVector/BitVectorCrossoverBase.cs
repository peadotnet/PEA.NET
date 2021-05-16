using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public abstract class BitVectorCrossoverBase : BitVectorOperationBase, ICrossover<BitVectorChromosome>
	{
		protected BitVectorCrossoverBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
			: base(random, parameterSet, conflictDetectors)
		{
		}

		public abstract IList<IChromosome> Cross(IChromosome parents0, IChromosome paren1);
	}
}
