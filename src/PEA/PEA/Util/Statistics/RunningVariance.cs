using System;

namespace Pea.Util
{
    public class RunningVariance : MeanAndDeviation
    {
        internal long Count = 0L;
        internal double SumOfSquareDifferences = 0.0;

        public RunningVariance() : base(0, 0)
        {
        }

        public RunningVariance(double mean, double deviation) : base(mean, deviation)
        {
        }

        public override double Deviation
        {
            get
            {
                if (!_deviation.HasValue)
                {
                    _deviation = Math.Sqrt(GetVarianceUnbiased());
                }
                return _deviation.Value;
            }
        }

        public void Add(double x)
        {
            Count++;
            double nextMean = Mean + (x - Mean) / Count;
            SumOfSquareDifferences += (x - Mean) * (x - nextMean);
            Mean = nextMean;
            _deviation = null;
        }

        public void Remove(double x)
        {
            if (Count == 0)
            {
                throw new ApplicationException("Cannot remove the sample. The number of samples is 0!");
            }
            else if (Count == 1)
            {
                Count = 0;
                Mean = 0.0;
                SumOfSquareDifferences = 0.0;
                _deviation = 0.0;
            }
            else
            {
                double mMOld = (Count * Mean - x) / (Count - 1);
                var squareDifference = (x - Mean) * (x - mMOld);
                SumOfSquareDifferences -= squareDifference;
                Mean = mMOld;
                Count--;
                _deviation = null;
            }
        }

        public double GetMean()
        {
            return Mean;
        }

        public double GetVarianceUnbiased()
        {
            return Count > 1 ? Math.Abs(SumOfSquareDifferences / (Count - 1)) : 0.0;
        }
    }
}
