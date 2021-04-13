using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class DoNothingCrossover : DoubleVectorOperatorBase, ICrossover<DoubleVectorChromosome>
	{
		public DoNothingCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) : base(random, parameterSet, conflictDetectors)
		{
		}

		public IList<IChromosome> Cross(IList<IChromosome> parents)
		{
			var offspring = new List<IChromosome>();
			for(int i=0; i< parents.Count; i++)
			{
				offspring.Add(parents[i].DeepClone());
			}
			return offspring;
		}
	}
}
