using System.Collections.Generic;
using Pea.Core;
using Pea.Util;
using Pea.Util.Statistics;

namespace Pea.Population
{
	public class Population : IPopulation
    {
        public IList<IEntity> Bests { get; set; } = new List<IEntity>();
        public int MaxNumberOfEntities { get; set; }
        public int MinNumberOfEntities { get; set; }
		private IList<IPopulationEntity> Entities { get; set; }
        public int Count => Entities.Count;
        public IStatisticsArray FitnessStatistics { get; }

        int badCounter = 0;

        public IEntity this[int index]
		{
            get
			{
                Entities[index].IndexInPopulation = index;
                return Entities[index];
            }
        }

        public IPopulation CloneEmpty()
        {
            return new Population(MinNumberOfEntities, MaxNumberOfEntities, FitnessStatistics.Length);
        }

        public Population(int fitnessLength, int minNumberOfEntities, int maxNumberOfEntities)
		{
            MinNumberOfEntities = minNumberOfEntities;
            MaxNumberOfEntities = maxNumberOfEntities;
            Entities = new List<IPopulationEntity>(maxNumberOfEntities);
            FitnessStatistics = new StatisticsArray(fitnessLength);
        }

        public void AddRange(EntityList entityList)
        {
            for(int i = 0; i < entityList.Count; i++)
            {
                Add(entityList[i]);
            }
        }

        public void Add(IEntity entity)
        {
            Entities.Add((IPopulationEntity)entity);
            FitnessStatistics.Add(entity.Fitness?.Value);
        }

		public void Remove(IEntity entity)
		{
            FitnessStatistics.Remove(entity.Fitness.Value);
            Entities.RemoveAt(((IPopulationEntity)entity).IndexInPopulation); 
		}

        public void RemoveAt(int index)
		{
            FitnessStatistics.Remove(Entities[index].Fitness.Value);
            Entities.RemoveAt(index);
        }

        public void Replace(IEntity entity)
        {
            var index = entity.IndexInList;
            FitnessStatistics.Remove(Entities[index].Fitness.Value);
            Entities[index] = entity as IPopulationEntity;
            FitnessStatistics.Add(entity.Fitness.Value);
        }

        public void Sort(IComparer<IEntity> comparer)
        {
            var sorter = new QuickSorter<IPopulationEntity>();
            sorter.Sort(Entities, comparer, 0, Entities.Count - 1);
        }

        public IEnumerator<IEntity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}
