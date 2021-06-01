using System.Collections.Generic;
using Pea.Core;

namespace Pea.Population
{
    public class Population : IPopulation
    {
        public IList<IEntity> Bests { get; set; } = new List<IEntity>();
        public int MaxNumberOfEntities { get; set; }
        public int MinNumberOfEntities { get; set; }
        public IList<IEntity> Entities { get; set; }

        public Population(int minNumberOfEntities, int maxNumberOfEntities)
		{
            MinNumberOfEntities = minNumberOfEntities;
            MaxNumberOfEntities = maxNumberOfEntities;
            Entities = new List<IEntity>(maxNumberOfEntities);
        }

        public void Add(IEntity entity)
        {
            Entities.Add(entity);
        }
    }
}
