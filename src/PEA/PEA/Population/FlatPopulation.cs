using System.Collections.Generic;
using Pea.Core;
using Pea.Core.Entity;
using Pea.Util;
using Pea.Util.Statistics;

namespace Pea.Population
{
	public class FlatPopulation : IPopulation
    {
        public IList<EntityBase> Bests { get; set; } = new List<EntityBase>();
        public int MaxNumberOfEntities { get; set; }
        public int MinNumberOfEntities { get; set; }
		private IList<EntityBase> Entities { get; set; }
        public int Count => Entities.Count;
        public IStatisticsArray FitnessStatistics { get; }


        public EntityBase this[int index]
		{
            get
			{
                return Entities[index];
            }
        }

        public IPopulation CloneEmpty()
        {
            return new FlatPopulation(MinNumberOfEntities, MaxNumberOfEntities, FitnessStatistics.Length);
        }

        public FlatPopulation(int fitnessLength, int minNumberOfEntities, int maxNumberOfEntities)
		{
            MinNumberOfEntities = minNumberOfEntities;
            MaxNumberOfEntities = maxNumberOfEntities;
            Entities = new List<EntityBase>(maxNumberOfEntities);
            FitnessStatistics = new StatisticsArray(fitnessLength);
        }

        public void AddRange(EntityList entityList)
        {
            for(int i = 0; i < entityList.Count; i++)
            {
                Add(entityList[i]);
            }
        }

        public void Add(EntityBase entity)
        {
            entity.IndexInPopulation = Entities.Count;
            Entities.Add((EntityBase)entity);
            FitnessStatistics.Add(entity.Fitness?.Value);
        }

		public void Remove(EntityBase entity)
		{
            var lastIndex = Entities.Count - 1;
            var lastEntity = Entities[lastIndex];
            lastEntity.IndexInPopulation = entity.IndexInPopulation;
            Entities[entity.IndexInPopulation] = lastEntity;
            Entities.RemoveAt(lastIndex);
            FitnessStatistics.Remove(entity.Fitness.Value);
        }

        public void RemoveAt(int index)
		{
            var entity = Entities[index];
            var lastIndex = Entities.Count - 1;
            var lastEntity = Entities[lastIndex];
            lastEntity.IndexInPopulation = index;
            Entities[index] = lastEntity;
            FitnessStatistics.Remove(entity.Fitness.Value);
            Entities.RemoveAt(lastIndex);
        }

        public void Replace(EntityBase entity)
        {
            var index = entity.IndexInPopulation;
            FitnessStatistics.Remove(Entities[index].Fitness.Value);
            Entities[index] = entity;
            FitnessStatistics.Add(entity.Fitness.Value);
        }

        public void Replace(int indexToReplace, EntityBase newEntity)
        {
            var entityToReplace = Entities[indexToReplace];
            FitnessStatistics.Remove(entityToReplace.Fitness.Value);
            newEntity.IndexInPopulation = indexToReplace;
            Entities[indexToReplace] = newEntity;
            FitnessStatistics.Add(newEntity.Fitness.Value);
        }

        public void Sort(IComparer<EntityBase> comparer)
        {
            var sorter = new QuickSorter<EntityBase>();
            sorter.Sort(Entities, comparer, 0, Entities.Count - 1);
            for (int i = 0; i < Entities.Count; i++)
            {
                Entities[i].IndexInPopulation = i;
            }
        }

        public IEnumerator<EntityBase> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }
    }
}
