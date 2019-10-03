using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(IEngine engine);
        IList<Type> GetSelections();
        IList<Type> GetReinsertions();
        IEnumerable<KeyValuePair<string, double>> GetParameters();
    }
}
