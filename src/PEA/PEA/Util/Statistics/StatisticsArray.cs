using System.Collections.Generic;

namespace Pea.Util.Statistics
{
	public class StatisticsArray : IStatisticsArray
	{
		public int Length { get; }
		public RunningVariance[] StatisticVariables { get; }

		public StatisticsArray(int length)
		{
			Length = length;
			StatisticVariables = new RunningVariance[length];
			for (int i = 0; i < length; i++)
			{
				StatisticVariables[i] = new RunningVariance();
			}
		}

		public void Add(IReadOnlyList<double> values)
		{
			if (values == null) throw new System.NullReferenceException(nameof(values));

			for (int i = 0; i < StatisticVariables.Length; i++)
			{
				StatisticVariables[i].Add(values[i]);
			}
		}

		public void Remove(IReadOnlyList<double> values)
		{
			if (values == null) throw new System.NullReferenceException(nameof(values));

			for (int i = 0; i < StatisticVariables.Length; i++)
			{
				StatisticVariables[i].Remove(values[i]);
			}
		}
	}
}
