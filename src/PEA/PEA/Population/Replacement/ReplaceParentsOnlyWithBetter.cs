using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
	public class ReplaceParentsOnlyWithBetter : ReplacementBase
	{
		public ReplaceParentsOnlyWithBetter(IRandom random, IFitnessComparer fitnessComparer,
				ParameterSet parameters) : base(random, fitnessComparer, parameters)
		{

		}

		public override IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
		{
			var inserted = new List<IEntity>();
			//TODO: FitnessComparer.SelectWorst, SelectBest

			if (offspring.Count == 0) return inserted;

			var entityToRemove = FitnessComparer.Dominates(parents[0].Fitness, parents[1].Fitness) ? parents[0] : parents[1];
			var entityToAdd = (offspring.Count > 1 && FitnessComparer.Dominates(offspring[0].Fitness, offspring[1].Fitness)) ? offspring[1] : offspring[0];

			if (FitnessComparer.Dominates(entityToRemove.Fitness, entityToAdd.Fitness))
			{
				RemoveEntitiesFromPopulation(sourcePopulation, new List<IEntity>() { entityToRemove });
				AddEntitiesToPopulation(targetPopulation, new List<IEntity>() { entityToAdd });
				inserted.Add(entityToAdd);
			}

			return inserted;
		}
	}
}
