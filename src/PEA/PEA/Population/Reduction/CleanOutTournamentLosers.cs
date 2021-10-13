using Pea.Core;
using Pea.Util;
using System;
using System.Collections.Generic;

namespace Pea.Population.Reduction
{
	public class CleanOutTournamentLosers : IReduction
	{
		protected IRandom Random { get; }
		protected IParameterSet ParameterSet { get; }

		public CleanOutTournamentLosers(IRandom random, IParameterSet parameterSet)
		{
			Random = random;
			ParameterSet = parameterSet;
		}

		public IPopulation Reduct(IPopulation population)
		{
			var count = population.Count;
			var reductionRate = ParameterSet.GetValue(ParameterNames.ReductionRate);
			int resultCount = Convert.ToInt32(population.Count * reductionRate);

			population.Sort(new TournamentLoserComparer());

			for (int c = count-1; c > resultCount; c--)
			{
				population.RemoveAt(c);
			}
			return population;
		}
	}
}
