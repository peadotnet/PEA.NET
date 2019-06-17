using Pea.Core;
using System.Collections.Generic;

namespace Pea.Reinsertion
{
    public class ReplaceParentsReinsertion : ReinsertionBase
    {
        public override void Reinsert(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            RemoveEntitiesFromPopulation(sourcePopulation, parents);
            AddEntitiesToPopulation(targetPopulation, offspring);
        }
    }
}
