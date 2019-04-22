using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation
    {
        List<IEntity> Entities { get; }
    }
}
