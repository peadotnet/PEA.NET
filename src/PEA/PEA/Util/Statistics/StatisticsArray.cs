using System.Collections.Generic;

namespace Pea.Util.Statistics
{
	public class StatisticsArray : IStatisticsArray
	{
		public int Length { get; }
		public RunningVariance[] StatisticVariables { get; }

        public RunningVariance this[int index]
		{
			get 
			{ 
				return StatisticVariables[index]; 
			}
			set
			{
				StatisticVariables[index] = value;
			}
		}

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
			if (values == null) return; // throw new System.NullReferenceException(nameof(values));

			for (int i = 0; i < StatisticVariables.Length; i++)
			{
				if (double.IsNormal(values[i]))
				{
					StatisticVariables[i].Add(values[i]);
				}
				else
				{
					break;
				}
			}
		}

		public void Remove(IReadOnlyList<double> values)
		{
			if (values == null) throw new System.NullReferenceException(nameof(values));

			for (int i = 0; i < StatisticVariables.Length; i++)
			{
                if (double.IsNormal(values[i]))
                {
                    StatisticVariables[i].Remove(values[i]);

                }
				else
				{
					break;
				}
            }
		}

        public StatisticsArray Clone()
		{
			var clone = new StatisticsArray(Length);
			CopyTo(this, clone);
			return clone;
		}


		public static void CopyTo(IStatisticsArray source, IStatisticsArray target)
		{
			for (int i = 0; i < source.Length; i++)
			{
				source[i].CopyTo(target[i]);
			}
		}

	}
}
