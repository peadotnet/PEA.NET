using Pea.Core;

namespace Pea.Population.Replacement
{
	public class ReplaceParentsOnlyWithBetter : ReplacementBase
	{
		public ReplaceParentsOnlyWithBetter(IRandom random, IFitnessComparer fitnessComparer,
				ParameterSet parameters) : base(random, fitnessComparer, parameters)
		{

		}

		public override IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation)
		{
			var inserted = new EntityList(1);
			var populationSize = Parameters.GetValue(Algorithm.ParameterNames.PopulationSize);
			//TODO: FitnessComparer.SelectWorst, SelectBest

			if (offspring.Count == 0) return inserted;

			var entityToRemove = FitnessComparer.Dominates(parents[0].Fitness, parents[1].Fitness) ? parents[0] : parents[1];
			var entityToAdd = (offspring.Count > 1 && FitnessComparer.Dominates(offspring[0].Fitness, offspring[1].Fitness)) ? offspring[1] : offspring[0];

			if (FitnessComparer.Dominates(entityToRemove.Fitness, entityToAdd.Fitness))
			{
				targetPopulation.Add(entityToAdd);
				inserted.Add(entityToAdd);

				if (sourcePopulation.Count > populationSize)
				{
					sourcePopulation.Remove(entityToRemove);
				}
			}

			return inserted;
		}
	}
}
