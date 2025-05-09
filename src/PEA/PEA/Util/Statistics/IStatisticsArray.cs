using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Pea.Util.Statistics
{
	public interface IStatisticsArray
	{
		int Length { get; }
		RunningVariance[] StatisticVariables { get; }

		RunningVariance this[int index] { get; set;}

		void Add(IReadOnlyList<double> values);
		void Remove(IReadOnlyList<double> values);
	}
}