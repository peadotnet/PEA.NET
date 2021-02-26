using System;
using System.Collections.Generic;
using Pea.Algorithm.Implementation;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Population.Replacement;

namespace Pea.Algorithm
{
    public class SteadyState : IAlgorithmFactory
    {
        public IAlgorithm GetAlgorithm(IEngine engine)
        {
            return new SteadyStateAlgorithm(engine);
        }

        public IEnumerable<PeaSettingsNamedValue> GetParameters()
        {
            return new List<PeaSettingsNamedValue>()
            {
                new PeaSettingsNamedValue(Selection.ParameterNames.TournamentSize, 2),
                new PeaSettingsNamedValue(Core.Island.ParameterNames.EvaluatorsCount, 2)
            };
        }

        public IList<Type> GetReinsertions()
        {
            return new List<Type>()
            {
                typeof(ReplaceParentsOnlyWithBetter)
                //typeof(ReplaceWorstParentWithBestChildrenReinsertion)
            };
        }

        public IList<Type> GetSelections()
        {
            return new List<Type>()
            {
                typeof(Selection.TournamentSelection)
            };
        }

    }
}
