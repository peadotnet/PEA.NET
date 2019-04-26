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
    }
}
