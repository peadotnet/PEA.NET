using System.Collections.Generic;

namespace Pea.Util
{
    public class MeanAndDeviation
    {
        public double Mean { get; set; }
        internal double? _deviation;

        public MeanAndDeviation(double mean, double deviation)
        {
            Mean = mean;
            _deviation = deviation;
        }

        public virtual double Deviation
        {
            get
            {
                return _deviation ?? 0.0;
            }
        }

        public void CopyTo(MeanAndDeviation other)
        {
            other.Mean = Mean;
            other._deviation = _deviation;
        }

        public virtual MeanAndDeviation Clone()
        {
            return new MeanAndDeviation(Mean, Deviation);
        }

        public class ComparerByMean : IComparer<RunningVariance>
        {
            private ComparerByMean() { }

            private static ComparerByMean _instance;
            public static ComparerByMean Instance
            {
                get
                {
                    if (_instance == null) _instance = new ComparerByMean();
                    return _instance;
                }
            }

            public int Compare(RunningVariance x, RunningVariance y)
            {
                return x.Mean.CompareTo(y.Mean);
            }
        }

        public class ComparerByDeviation : IComparer<RunningVariance>
        {
            private ComparerByDeviation() { }

            private static ComparerByDeviation _instance;

            public static ComparerByDeviation Instance
            {
                get
                {
                    if (_instance == null) _instance = new ComparerByDeviation();
                    return _instance;
                }
            }

            public int Compare(RunningVariance x, RunningVariance y)
            {
                return x.Deviation.CompareTo(y.Deviation);
            }
        }
    }
}
