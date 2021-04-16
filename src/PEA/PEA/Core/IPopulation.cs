using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation
    {
        IList<IEntity> Bests { get; }
        int MaxNumberOfEntities { get; set; }
        int MinNumberOfEntities { get; set; }
        IList<IEntity> Entities { get; }
        void Add(IEntity entity);
    }
}
