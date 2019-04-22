using System;

namespace Pea.Core
{
    public class PredeterminedRandom : IRandom
    {
        public int[] IntValues { get; set; }
        public double[] DoubleValues { get; set; }

        int _index = 0;

        public PredeterminedRandom(params int[] intValues)
        {
            IntValues = intValues;
        }

        public PredeterminedRandom(params double[] doubleValues)
        {
            DoubleValues = doubleValues;
        }

        public int GetInt(int minValue, int maxValue)
        {
            int value = IntValues[_index++ % IntValues.Length];
            value = value % (maxValue - minValue + 1);
            return value + minValue;
        }

        public double GetDouble(double minValue, double maxValue)
        {
            if (Math.Abs(minValue - maxValue) < double.Epsilon) return minValue;

            double value = DoubleValues[_index++ % DoubleValues.Length];
            var divider = maxValue - minValue;

            value = value - Math.Floor(value / divider) * divider + minValue;
            return value;
        }
    }
}
