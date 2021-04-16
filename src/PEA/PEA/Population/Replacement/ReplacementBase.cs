using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
    public abstract class ReplacementBase : IReplacement
    {
        public IRandom Random { get; }
        public IFitnessComparer FitnessComparer { get; }
        public ParameterSet Parameters { get; }

        protected ReplacementBase(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters)
        {
            Random = random;
            FitnessComparer = fitnessComparer;
            Parameters = parameters;
        }

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

        public abstract IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation);
    }
}
