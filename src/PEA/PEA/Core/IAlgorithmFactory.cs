using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(IEngine engine);
        IList<Type> GetSelections();
        IList<Type> GetReinsertions();
        IEnumerable<PeaSettingsNamedValue> GetParameters();
    }
}
