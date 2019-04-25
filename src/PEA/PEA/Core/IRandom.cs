namespace Pea.Core
{
    public interface IRandom
    {
        int GetInt(int minValue, int upperBound);

        int GetIntWithTabu(int minValue, int upperBound, int tabu);

        double GetDouble(double minValue, double upperBound);
    }
}

