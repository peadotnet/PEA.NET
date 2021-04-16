using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population.Replacement
{
    public class ReplaceParentsReinsertion : ReplacementBase
    {
        public ReplaceParentsReinsertion(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters)
        : base(random, fitnessComparer, parameters)
        {
        }

        public override IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            RemoveEntitiesFromPopulation(sourcePopulation, parents);
            AddEntitiesToPopulation(targetPopulation, offspring);
            return offspring;
        }
    }
}
