 using System;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public abstract class RandomBase : IRandom
    {
        public abstract int GetInt(int minValue, int upperBound);

        public abstract double GetDouble(double minValue, double upperBound);

        protected RandomBase(int seed)
        {

        }

        public virtual int GetIntWithTabu(int minValue, int upperBound, params int[] tabu)
        {
            var result = tabu[0];
            while (tabu.Contains(result))
            {
                result = GetInt(minValue, upperBound);
            }

            return result;
        }

        public virtual IList<int> GetUniqueInts(int minValue, int upperBound, int count)
        {
            var diff = upperBound - minValue;

            if (diff < count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var ints = new List<int>();

            for (int i = 0; i < count; i++)
            {
                var value = GetInt(minValue, upperBound);
                while (ints.Contains(value))
                {
                    value = GetInt(minValue, upperBound);
                }
                ints.Add(value);
            }

            return ints;
        }

        public virtual double GetGaussian(double mean, double deviation)
        {
            double x1 = 1 - GetDouble(0, 1);
            double x2 = 1 - GetDouble(0, 1);    //TODO: buffer one ?

            double y1 = deviation * Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return y1 + mean;
        }
    }
}
