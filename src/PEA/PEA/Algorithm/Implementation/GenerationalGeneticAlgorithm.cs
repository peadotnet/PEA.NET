using Pea.Core;
using System;

namespace Pea.Algorithm.Implementation
{
	public class GenerationalGeneticAlgorithm : GeneticAlgorithmBase
	{
		public GenerationalGeneticAlgorithm(ParameterSet parameters, IProvider<IEntityCreator> entityCreators, Action<IEntityList> mergeToBests) : base(parameters, entityCreators, mergeToBests)
		{
		}

		public override StopDecision RunOnce()
		{
			var populationSize = Parameters.GetInt(ParameterNames.PopulationSize);
			var selectionRate = Parameters.GetValue(ParameterNames.SelectionRate);
			var minEntityCount = Convert.ToInt32(selectionRate * populationSize);

			var nextGeneration = Population.CloneEmpty();
			var parents = SelectParents(Population, minEntityCount);
			var offspring = Crossover(parents, populationSize);
			var mutated = Mutate(offspring);
			var evaluated = Evaluate(mutated);
			//TODO: Niching ?
			var inserted = Reinsert(nextGeneration, evaluated, parents, Population);
			MergeToBests(inserted);
			return StopCriteria.MakeDecision(Population, FitnessComparer);
		}
	}
}
