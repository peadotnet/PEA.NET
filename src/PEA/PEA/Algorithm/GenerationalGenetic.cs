using Pea.Algorithm.Implementation;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Population.Replacement;
using System;
using System.Collections.Generic;

namespace Pea.Algorithm
{
	public class GenerationalGenetic : IAlgorithmFactory
	{
		public IAlgorithm GetAlgorithm(IEngine engine)
		{
			return new GenerationalGeneticAlgorithm(engine);
		}

		public IEnumerable<PeaSettingsNamedValue> GetParameters()
		{
			return new List<PeaSettingsNamedValue>()
			{
				new PeaSettingsNamedValue(Selection.ParameterNames.TournamentSize, 4),
				new PeaSettingsNamedValue(Core.Island.ParameterNames.EvaluatorsCount, 8),
				new PeaSettingsNamedValue(ParameterNames.SelectionRate, 0.2)
			};
		}

		public IList<Type> GetReinsertions()
		{
			return new List<Type>()
			{
				typeof(ReinsertAll)
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
