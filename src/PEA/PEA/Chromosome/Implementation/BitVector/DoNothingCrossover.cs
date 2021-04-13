using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class DoNothingCrossover : BitVectorCrossoverBase
	{
		public DoNothingCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
			: base(random, parameterSet, conflictDetectors)
		{
		}

		public override IList<IChromosome> Cross(IList<IChromosome> parents)
		{
			var offspring = new List<IChromosome>();
			for (int i = 0; i < parents.Count; i++)
			{
				offspring.Add(parents[i].DeepClone());
			}
			return offspring;
		}
	}
}
