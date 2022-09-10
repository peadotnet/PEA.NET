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
            return new List<PeaSettingsNamedValue>(3)
            {
                new PeaSettingsNamedValue(Population.ParameterNames.TournamentSize, 4),
                new PeaSettingsNamedValue(Core.Island.ParameterNames.EvaluatorsCount, 2),
                new PeaSettingsNamedValue(ParameterNames.SubCycleRunCount, 100000),
                new PeaSettingsNamedValue(Population.ParameterNames.ReductionRate, 0.9),
                new PeaSettingsNamedValue(ParameterNames.SelectionRate, 0.95)
            };
        }

        public IList<Type> GetReinsertions()
        {
            return new List<Type>(1)
            {
				typeof(ReplaceParentsOnlyWithBetter),
				//typeof(ReplaceWorstParentWithBestChildrenReinsertion)
			};
        }

        public IList<Type> GetSelections()
        {
            return new List<Type>(1)
            {
                typeof(Selection.TournamentSelection)
            };
        }

    }
}
