using System.Collections.Generic;

namespace Pea.Core
{
    public interface IRandom
    {
        int GetInt(int minValue, int upperBound);

        int GetIntWithTabu(int minValue, int upperBound, params int[] tabu);

        IList<int> GetUniqueInts(int minValue, int upperBound, int count);

        double GetDouble(double minValue, double upperBound);

        double GetGaussian(double mean, double deviation);
    }
}

