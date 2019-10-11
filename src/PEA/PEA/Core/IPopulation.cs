using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation
    {
        IList<IEntity> Bests { get; set; }
        int MaxNumberOfEntities { get; set; }
        int MinNumberOfEntities { get; set; }
        IList<IEntity> Entities { get; set; }
        void Add(IEntity entity);
    }
}
