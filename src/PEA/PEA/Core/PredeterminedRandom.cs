using System;
using System.Linq;

namespace Pea.Core
{
    public class PredeterminedRandom : RandomBase, IRandom
    {
        public int[] IntValues { get; set; }
        public double[] DoubleValues { get; set; }

        private int _index = 0;

        public PredeterminedRandom(params double[] doubleValues) : base(0)
        {
            DoubleValues = doubleValues;
            IntValues = doubleValues.Select(Convert.ToInt32).ToArray();
        }

        public override int GetInt(int minValue, int upperBound)
        {
            if (minValue == upperBound) return minValue;

            int value = IntValues[_index++ % IntValues.Length];
            value = value % (upperBound - minValue);
            return value + minValue;
        }

        public override double GetDouble(double minValue, double upperBound)
        {
            if (Math.Abs(minValue - upperBound) < double.Epsilon) return minValue;

            double value = DoubleValues[_index++ % DoubleValues.Length];
            var divider = upperBound - minValue;

            value = value - Math.Floor(value / divider) * divider + minValue;
            return value;
        }
    }
}
