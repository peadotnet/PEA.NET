using System;

namespace Pea.Core
{
    public class SystemRandom : IRandom
    {
        private readonly Random _random;

        public SystemRandom()
        {
            _random = new Random(Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds));
        }

        public double GetDouble(double minValue, double maxValue)
        {
            return _random.NextDouble() * (maxValue - minValue) + minValue;
        }

        public int GetInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
