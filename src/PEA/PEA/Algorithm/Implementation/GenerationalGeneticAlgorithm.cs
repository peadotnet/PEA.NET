using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Algorithm.Implementation
{
	public class GenerationalGeneticAlgorithm : GeneticAlgorithmBase
	{
		public GenerationalGeneticAlgorithm(Core.IEngine engine) : base(engine)
		{
		}

		public override void InitPopulation()
		{
			Population = new Population.Population();

			var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
			for (int i = 0; i < maxNumberOfEntities; i++)
			{
				var entity = CreateEntity();
				Population.Add(entity);
			}

			Evaluate(Population.Entities);
			MergeToBests(Population.Entities);
		}

		public override void RunOnce()
		{
			var selectionRate = Engine.Parameters.GetInt(ParameterNames.SelectionRate);
			var minEntityCount = selectionRate * Population.Entities.Count;

			var nextGeneration = new List<IEntity>();
			var parents = SelectParents(Population.Entities, minEntityCount);
			var offspring = Crossover(parents, minEntityCount);
			var mutated = Mutate(offspring);
			var evaluated = Evaluate(mutated);
			//TODO: Niching ?
			var inserted = Reinsert(nextGeneration, evaluated, parents, Population.Entities);
			MergeToBests(inserted);
			Population.Entities = nextGeneration;
		}
	}
}
