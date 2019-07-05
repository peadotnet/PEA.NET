using Pea.Core;
using System.Collections.Generic;

namespace Pea.Reinsertion
{
    public class ReplaceParentsReinsertion : ReinsertionBase
    {
        public ReplaceParentsReinsertion(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters)
        : base(random, fitnessComparer, parameters)
        {
        }

        public override void Reinsert(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            RemoveEntitiesFromPopulation(sourcePopulation, parents);
            AddEntitiesToPopulation(targetPopulation, offspring);
        }
    }
}
