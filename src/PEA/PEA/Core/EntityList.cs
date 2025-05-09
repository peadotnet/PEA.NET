using System.Collections.Generic;

namespace Pea.Core
{
	public class EntityList : IEntityList
	{
		private IList<IEntity> Entities { get; set; }
		public int Count => Entities.Count;
		public IEntity this[int index]
		{
			get
			{
				Entities[index].IndexInList = index;
				return Entities[index];
			}
		}

		public EntityList(int count)
		{
			Entities = new List<IEntity>(count);
		}

		public EntityList(ICollection<IEntity> entities)
		{
			var entityList = new List<IEntity>(entities.Count);
			entityList.AddRange(entities);
			Entities = entityList;
		}

		public void AddRange(IList<IEntity> entities)
		{
			for (int i = 0; i < entities.Count; i++)
			{
				Entities.Add(entities[i]);
			}
		}

		public void AddRange(EntityList entities)
		{
			AddRange(entities.Entities);
		}

		public void Add(IEntity entity)
		{
			Entities.Add(entity);
		}

		public void Remove(IEntity entity)
		{
			Entities.Remove(entity);
		}

		public void RemoveAt(int index)
		{
			Entities.RemoveAt(index);
		}

		public IEnumerator<IEntity> GetEnumerator()
		{
			return Entities.GetEnumerator();
		}

		public void Replace(IEntity entity)
		{
			var index = entity.IndexInList;
			Entities[index] = entity;
		}
	}
}
