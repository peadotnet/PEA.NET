using System;

namespace Pea.Core
{
    /// <summary>
    /// IRandom implementation using FastRandom has pseudo-number generator.
    /// <see href="http://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random"/>
    /// </summary>
    public class FastRandom : RandomBase, IRandom
    {
        private readonly SharpNeatLib.Maths.FastRandom _random;

        public FastRandom(int seed) : base(seed)
        {
            _random = new SharpNeatLib.Maths.FastRandom(seed);
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
