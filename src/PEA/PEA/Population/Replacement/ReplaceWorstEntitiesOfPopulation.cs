using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
    public class ReplaceWorstEntitiesOfPopulation : ReplacementBase
    {
        public ReplaceWorstEntitiesOfPopulation(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters) : base(random, fitnessComparer, parameters)
        {
        }

        public override void Replace(IList<IEntity> population, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            int tournamentSize = Parameters.GetInt(ParameterNames.TournamentSize);

            for (int i = 0; i < offspring.Count; i++)
            {
                var entityForRemove = SelectOne(population, tournamentSize);
                population.RemoveAt(entityForRemove);
                population.Add(offspring[i]);
            }
        }

        private int SelectOne(IList<IEntity> entities, int size)
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
