using Pea.Core;
using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Population.Reduction
{
	public class TournamentLoserComparer : IComparer<EntityBase>
	{
		public int Compare(EntityBase x, EntityBase y)
		{
			return x.Fitness.TournamentLoser.CompareTo(y.Fitness.TournamentLoser);
		}
	}
}
