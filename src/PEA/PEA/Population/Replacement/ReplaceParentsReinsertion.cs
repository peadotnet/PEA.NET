using Pea.Core;

namespace Pea.Population.Replacement
{
	public class ReplaceParentsReinsertion : ReplacementBase
    {
        public ReplaceParentsReinsertion(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters)
            : base(random, fitnessComparer, parameters)
        {
        }

        public override IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation)
        {
            RemoveEntitiesFromPopulation(sourcePopulation, parents);
            AddEntitiesToPopulation(targetPopulation, offspring);
            return offspring;
        }
    }
}
