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

			var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.MaxNumberOfEntities);
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
			var minCount = Engine.Parameters.GetInt(ParameterNames.NumberOfSelectedEntities);

			var parents = SelectParents(Population.Entities, minCount);
			var offspring = Crossover(parents, minCount);
			var mutated = Mutate(offspring);
			var evaluated = Evaluate(mutated);

			if (evaluated[0] == null)
			{
				bool brk = true;
			}
			//TODO: Reduction (children) ?
			var inserted = Reinsert(Population.Entities, evaluated, parents, Population.Entities);
			MergeToBests(inserted);
		}
	}
}
