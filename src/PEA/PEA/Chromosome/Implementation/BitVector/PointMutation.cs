using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class PointMutation : BitVectorMutationBase
	{
		public PointMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
			: base(random, parameterSet, conflictDetectors)
		{
		}

		public override BitVectorChromosome Mutate(BitVectorChromosome chromosome)
		{
			var result = chromosome.DeepClone();
			var pos = Random.GetInt(0, chromosome.Genes.Length);
			chromosome.Genes[pos] = !chromosome.Genes[pos];
			return result;
		}
	}
}
