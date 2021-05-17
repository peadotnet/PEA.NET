using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Population.Replacement
{
	public class ReinsertAll : ReplacementBase
	{
		public ReinsertAll(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters) : base(random, fitnessComparer, parameters)
		{
		}

		public override IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
		{
			AddEntitiesToPopulation(targetPopulation, offspring);
			return offspring;
		}
	}
}
