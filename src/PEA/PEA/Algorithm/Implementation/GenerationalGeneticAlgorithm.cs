using Akka.Event;
using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pea.Algorithm.Implementation
{
	public class GenerationalGeneticAlgorithm : GeneticAlgorithmBase
	{
		public GenerationalGeneticAlgorithm(IEngine engine) : base(engine)
		{
		}

		public override StopDecision RunOnce()
		{
			var populationSize = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
			var selectionRate = Engine.Parameters.GetValue(ParameterNames.SelectionRate);
			var minEntityCount = Convert.ToInt32(selectionRate * populationSize);

			var nextGeneration = Population.CloneEmpty();
			var parents = SelectParents(Population, minEntityCount);
			var offspring = Crossover(parents, populationSize);
			var mutated = Mutate(offspring);
			var evaluated = Evaluate(mutated);
			//TODO: Niching ?
			var inserted = Reinsert(nextGeneration, evaluated, parents, Population);
			MergeToBests(inserted);
			return StopCriteria.MakeDecision(Engine, Population);
		}
	}
}
