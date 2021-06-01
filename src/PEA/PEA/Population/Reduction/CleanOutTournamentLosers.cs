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

		public IList<IEntity> Reduct(IList<IEntity> entities)
		{
			var count = entities.Count;
			var reductionRate = ParameterSet.GetValue(ParameterNames.ReductionRate);
			int resultCount = Convert.ToInt32(entities.Count * reductionRate);

			var sorter = new QuickSorter<IEntity>();
			sorter.Sort(entities, new TournamentLoserComparer(), 0, entities.Count - 1);

			for (int c = count-1; c > resultCount; c--)
			{
				entities.RemoveAt(c);
			}
			return entities;
		}
	}
}
