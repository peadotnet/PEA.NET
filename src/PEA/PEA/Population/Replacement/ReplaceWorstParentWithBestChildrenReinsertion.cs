using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
    public class ReplaceWorstParentWithBestChildrenReinsertion : ReplacementBase
    {
        public ReplaceWorstParentWithBestChildrenReinsertion(IRandom random, IFitnessComparer fitnessComparer,
            ParameterSet parameters) : base(random, fitnessComparer, parameters)
        {

        }

        public override IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation)
        {
            //TODO: FitnessComparer.SelectWorst, SelectBest

            var populationSize = Parameters.GetInt(Algorithm.ParameterNames.PopulationSize);
            var inserted = new EntityList(offspring.Count);

            if (offspring.Count == 0) return inserted;

            var entityToRemove = FitnessComparer.Dominates(parents[0].Fitness, parents[1].Fitness) ? parents[0] : parents[1];

            var entityToAdd = (offspring.Count > 1 && FitnessComparer.Dominates(offspring[0].Fitness, offspring[1].Fitness)) ? offspring[1] : offspring[0];

            if (!entityToAdd.Fitness.IsLethal())
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
