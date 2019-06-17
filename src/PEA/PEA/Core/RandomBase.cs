using System;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public abstract class RandomBase : IRandom
    {
        public abstract int GetInt(int minValue, int upperBound);

        public abstract double GetDouble(double minValue, double upperBound);

        public virtual int GetIntWithTabu(int minValue, int upperBound, int tabu)
        {
            var result = tabu;
            while (result == tabu)
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
    }
}
