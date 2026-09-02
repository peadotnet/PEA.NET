using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(ParameterSet parameters, IProvider<IEntityCreator> entityCreators, Action<IEntityList> mergeToBests);
        IList<Type> GetSelections();
        IList<Type> GetReinsertions();
        IEnumerable<PeaSettingsNamedValue> GetParameters();
    }
}
