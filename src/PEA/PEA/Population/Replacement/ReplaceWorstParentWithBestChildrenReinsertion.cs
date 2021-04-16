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

        public override IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            //TODO: FitnessComparer.SelectWorst, SelectBest

            var inserted = new List<IEntity>();

            if (offspring.Count == 0) return inserted;

            var entityToRemove = FitnessComparer.Dominates(parents[0].Fitness, parents[1].Fitness) ? parents[0] : parents[1];

            RemoveEntitiesFromPopulation(sourcePopulation, new List<IEntity>() { entityToRemove });

            var entityToAdd = (offspring.Count > 1 && FitnessComparer.Dominates(offspring[0].Fitness, offspring[1].Fitness)) ? offspring[1] : offspring[0];

            AddEntitiesToPopulation(targetPopulation, new List<IEntity>() { entityToAdd });

            inserted.Add(entityToAdd);

            return inserted;
        }
    }
}
