using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population
{
    public class Population : IPopulation
    {
        public IList<IEntity> Bests { get; set; } = new List<IEntity>();
        public int MaxNumberOfEntities { get; set; }
        public int MinNumberOfEntities { get; set; }
        public IList<IEntity> Entities { get; set; } = new List<IEntity>();
        public void Add(IEntity entity)
        {
            Entities.Add(entity);
        }
    }
}
