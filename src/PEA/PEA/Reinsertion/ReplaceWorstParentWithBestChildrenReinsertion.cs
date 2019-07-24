using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Reinsertion
{
    public class ReplaceWorstParentWithBestChildrenReinsertion : ReinsertionBase
    {
        public ReplaceWorstParentWithBestChildrenReinsertion(IRandom random, IFitnessComparer fitnessComparer,
            ParameterSet parameters) : base(random, fitnessComparer, parameters)
        {

        }

        public override void Reinsert(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            //TODO: FitnessComparer.SelectWorst, SelectBest

            if (offspring.Count == 0) return;

            IEntity entity;
            if (FitnessComparer.Compare(parents[0].Fitness, parents[1].Fitness) > 0)
            {
                entity = parents[0];
            }
            else
            {
                entity = parents[1];
            }

            RemoveEntitiesFromPopulation(sourcePopulation, new List<IEntity>() { entity });

            entity = offspring[0];

            if (offspring.Count > 1 && FitnessComparer.Compare(offspring[0].Fitness, offspring[1].Fitness) > 0)
            {
                entity = offspring[1];
            }

            AddEntitiesToPopulation(targetPopulation, new List<IEntity>() { entity });
        }
    }
}
