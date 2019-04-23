using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation
    {
        IList<IEntity> Entities { get; }
    }
}
