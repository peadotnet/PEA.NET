using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Algorithm.Implementation
{
	public class GenerationalGeneticAlgorithm : GeneticAlgorithmBase
	{
		public GenerationalGeneticAlgorithm(IEngine engine) : base(engine)
		{
		}

		public override void InitPopulation()
		{
			var fitnessLength = Engine.Parameters.GetInt(ParameterNames.FitnessLength);
			var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
			var minNumberOfEntities = Convert.ToInt32(Engine.Parameters.GetValue(ParameterNames.SelectionRate) * maxNumberOfEntities);

			Population = new Population.Population(fitnessLength, minNumberOfEntities, maxNumberOfEntities);

			for (int i = 0; i < maxNumberOfEntities; i++)
			{
				var entity = CreateEntity();
				Population.Add(entity);
			}

			Evaluate(Population);
			MergeToBests(Population);
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
