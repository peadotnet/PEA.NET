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

        public virtual void RemoveEntitiesFromPopulation(IPopulation population, IEntityList entities)
        {
            for(int i=0; i < entities.Count; i++)
			{
                population.Remove(entities[i]);
			}
        }

        public virtual void AddEntitiesToPopulation(IPopulation population, IEntityList entities) 
        {
            for(int i=0; i< entities.Count; i++)
            {
                population.Add(entities[i]);
            }
        }

        public abstract IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation);
    }
}
