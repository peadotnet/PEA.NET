using System;
using Pea.Core;

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
