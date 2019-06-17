using System.Collections.Generic;

namespace Pea.Core
{
    public interface IReinsertion
    {
        void Reinsert(IList<IEntity> population, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation);
    }
}