using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public interface IAlgorithm
    {
        IEngine Engine { get; }
        IPopulation Population { get; set; }

        IList<Type> GetSelections();
        IList<Type> GetReinsertions();
        IEnumerable<PeaSettingsNamedValue> GetParameters();

        void InitPopulation();
        void RunOnce();
    }
}
