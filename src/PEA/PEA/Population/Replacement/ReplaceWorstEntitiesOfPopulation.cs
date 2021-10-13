using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
    public class ReplaceWorstEntitiesOfPopulation : ReplacementBase
    {
        public ReplaceWorstEntitiesOfPopulation(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters) : base(random, fitnessComparer, parameters)
        {
        }

        public override IEntityList Replace(IPopulation population, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation)
        {
            var inserted = new EntityList(offspring.Count);
            int tournamentSize = Parameters.GetInt(ParameterNames.TournamentSize);

            for (int i = 0; i < offspring.Count; i++)
            {
                var entityForRemove = SelectOne(population, tournamentSize);
                population.RemoveAt(entityForRemove);
                population.Add(offspring[i]);
                inserted.Add(offspring[i]);
            }

            return inserted;
        }

        private int SelectOne(IEntityList entities, int size)
        {
            var index = Random.GetInt(0, entities.Count);
            var worst = index;
            
            for (int i = 1; i < size; i++)
            {
                index = Random.GetInt(0, entities.Count);
                var comparisonResult = FitnessComparer.Compare(entities[worst].Fitness, entities[index].Fitness);
                if (comparisonResult < 0)
                {
                    worst = index;
                }
            }

            return worst;
        }
    }
}
