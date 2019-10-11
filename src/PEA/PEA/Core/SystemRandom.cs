using System;

namespace Pea.Core
{
    public class SystemRandom : RandomBase, IRandom
    {
        private readonly Random _random;

        public SystemRandom(int seed) : base(seed)
        {
            _random = new Random(seed);
        }

        public override double GetDouble(double minValue, double upperBound)
        {
            return _random.NextDouble() * (upperBound - minValue) + minValue;
        }

        public override int GetInt(int minValue, int upperBound)
        {
            return _random.Next(minValue, upperBound);
        }
    }
}
