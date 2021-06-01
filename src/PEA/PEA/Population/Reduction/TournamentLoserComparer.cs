using Pea.Core;
using System.Collections.Generic;

namespace Pea.Population.Reduction
{
	public class TournamentLoserComparer : IComparer<IEntity>
	{
		public int Compare(IEntity x, IEntity y)
		{
			return x.Fitness.TournamentLoser.CompareTo(y.Fitness.TournamentLoser);
		}
	}
}
