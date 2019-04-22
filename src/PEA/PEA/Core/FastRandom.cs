using System;

namespace Pea.Core
{
    /// <summary>
    /// An IRandomization using FastRandom has pseudo-number generator.
    /// <see href="http://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random"/>
    /// </summary>
    public class FastRandom : IRandom
    {
        readonly SharpNeatLib.Maths.FastRandom _random = new SharpNeatLib.Maths.FastRandom(DateTime.Now.Millisecond);

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
