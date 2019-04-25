using System;

namespace Pea.Core
{
    /// <summary>
    /// An IRandomization using FastRandom has pseudo-number generator.
    /// <see href="http://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random"/>
    /// </summary>
    public class FastRandom : RandomBase, IRandom
    {
        readonly SharpNeatLib.Maths.FastRandom _random = new SharpNeatLib.Maths.FastRandom(DateTime.Now.Millisecond);

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
