using Pea.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Reinsertion
{
    public abstract class ReinsertionBase : IReinsertion
    {
        public virtual void RemoveEntitiesFromPopulation(IList<IEntity> population, IList<IEntity> entities)
        {
            for (int i = population.Count - 1; i >= 0; i--)
            {
                if (entities.Contains(population[i]))
                {
                    population.RemoveAt(i);
                }
            }
        }

        public virtual void AddEntitiesToPopulation(IList<IEntity> population, IList<IEntity> entities)
        {
            foreach(var entity in entities)
            {
                population.Add(entity);
            }
        }

        public abstract void Reinsert(IList<IEntity> population, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation);
    }
}
