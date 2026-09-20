using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Core
{
	public class EntityList : IEntityList
	{
		private IList<EntityBase> Entities { get; set; }
		public int Count => Entities.Count;
		public EntityBase this[int index]
		{
			get
			{
				return Entities[index];
			}
		}

		public EntityList(int count)
		{
			Entities = new List<EntityBase>(count);
		}

		public EntityList(ICollection<EntityBase> entities)
		{
			var entityList = new List<EntityBase>(entities.Count);
			entityList.AddRange(entities);
            for (int i = 0; i < entityList.Count; i++)
			{
				entityList[i].IndexInList = i;
			}

            Entities = entityList;
		}

		public void AddRange(IList<EntityBase> entities)
		{
			for (int i = 0; i < entities.Count; i++)
			{
				Add(entities[i]);
			}
		}

		public void AddRange(EntityList entities)
		{
			AddRange(entities.Entities);
		}

		public void Add(EntityBase entity)
		{
			entity.IndexInList = Entities.Count;
			Entities.Add(entity);
		}

		public void Remove(EntityBase entity)
		{
			var lastIndex = Entities.Count - 1;
			var lastEntity = Entities[lastIndex];
			lastEntity.IndexInList = entity.IndexInList;
			Entities[entity.IndexInList] = lastEntity;
			Entities.RemoveAt(lastIndex);
		}

		public void RemoveAt(int index)
		{
			var lastIndex = Entities.Count - 1;
			var lastEntity = Entities[lastIndex];
            lastEntity.IndexInList = index;
            Entities[index] = lastEntity;
			Entities.RemoveAt(lastIndex);
		}

		public IEnumerator<EntityBase> GetEnumerator()
		{
			return Entities.GetEnumerator();
		}

		public void Replace(EntityBase entity)
		{
			var index = entity.IndexInList;
			Entities[index] = entity;
		}

        public void Replace(int indexToReplace, EntityBase newEntity)
        {
            newEntity.IndexInList = indexToReplace;
            Entities[indexToReplace] = newEntity;
        }
    }
}
