using System;
using System.Collections.Generic;
using Pea.Algorithm.Implementation;
using Pea.Core;

namespace Pea.Algorithm
{
    public class SteadyState : IAlgorithmFactory
    {
        public IAlgorithm GetAlgorithm(IEngine engine)
        {
            return new SteadyStateAlgorithm(engine);
        }

        public IEnumerable<KeyValuePair<string, double>> GetParameters()
        {
            return new List<KeyValuePair<string, double>>()
            {
                new KeyValuePair<string, double>(Selection.ParameterNames.TournamentSize, 2),
                new KeyValuePair<string, double>(Core.Island.ParameterNames.EvaluatorsCount, 2)
            };
        }

        public IList<Type> GetReinsertions()
        {
            return new List<Type>()
            {
                typeof(Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion)
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
