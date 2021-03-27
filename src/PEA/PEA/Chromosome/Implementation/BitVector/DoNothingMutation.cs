using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class DoNothingMutation : BitVectorMutationBase
	{
		public DoNothingMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
			: base(random, parameterSet, conflictDetectors)
		{
		}

		public override BitVectorChromosome Mutate(BitVectorChromosome chromosome)
		{
			return chromosome.DeepClone();
		}
	}
}
