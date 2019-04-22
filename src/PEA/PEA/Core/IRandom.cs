namespace Pea.Core
{
    public interface IRandom
    {
        int GetInt(int minValue, int maxValue);

        double GetDouble(double minValue, double maxValue);
    }
}

