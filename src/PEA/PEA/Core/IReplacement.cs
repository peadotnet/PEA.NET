using System.Collections.Generic;

namespace Pea.Core
{
    public interface IReplacement
    {
        void Replace(IList<IEntity> population, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation);
    }
}