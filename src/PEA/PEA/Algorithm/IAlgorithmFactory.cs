using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Entity;

namespace Pea.Algorithm
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(ParameterSet parameters, IProvider<IEntityCreator> entityCreators, Action<IEntityList> mergeToBests);
        IList<Type> GetSelections();
        IList<Type> GetReinsertions();
        IEnumerable<PeaSettingsNamedValue> GetParameters();
    }
}
