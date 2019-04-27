using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation
    {
        int MaxNumberOfEntities { get; set; }
        int MinNumberOfEntities { get; set; }
        IList<IEntity> Entities { get; set; }
        void Add(IEntity entity);
    }
}
