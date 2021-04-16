using System.Collections.Generic;

namespace Pea.Core
{
    public interface IReplacement
    {
        IList<IEntity> Replace(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation);
    }
}