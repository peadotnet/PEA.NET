using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.StructVector
{
	public class SwapTwoMutation<TS> : StructVectorOperatorBase<TS>, IMutation<StructChromosome<TS>> where TS: struct
	{
		public SwapTwoMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) : base(random, parameterSet, conflictDetectors)
		{
		}

		public IChromosome Mutate(IChromosome chromosome)
		{
			var genes = chromosome as StructChromosome<TS>;
			var length = genes.Genes.Length;

			var position1 = Random.GetInt(0, length);
			var position2 = Random.GetIntWithTabu(0, length, position1);

			var swap = genes.Genes[position2];
			genes.Genes[position2] = genes.Genes[position1];
			genes.Genes[position1] = swap;

			return genes;
		}
	}
}
