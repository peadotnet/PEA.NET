using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public abstract class BitVectorOperationBase
	{
		public IList<IConflictDetector> ConflictDetectors { get; set; }

		protected readonly IRandom Random;
		protected readonly IParameterSet ParameterSet;

		protected BitVectorOperationBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null)
		{
			Random = random;
			ParameterSet = parameterSet;
			ConflictDetectors = conflictDetectors;
		}



	}
}
