using System.Collections.Generic;

namespace Pea.Util.Statistics
{
	public interface IStatisticsArray
	{
		int Length { get; }
		RunningVariance[] StatisticVariables { get; }

		void Add(IReadOnlyList<double> values);
		void Remove(IReadOnlyList<double> values);
	}
}