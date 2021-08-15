using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.StructVector
{
	public class UniformMutation<TS> : StructVectorOperatorBase<TS>, IMutation<StructChromosome<TS>> where TS: struct
	{
		public UniformMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) : base(random, parameterSet, conflictDetectors)
		{
		}

		public IChromosome Mutate(IChromosome chromosome)
		{
			throw new NotImplementedException();
		}
	}
}
