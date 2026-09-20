using Pea.Core.Entity;

namespace Pea.Core
{
	public interface IEntityList
	{
		int Count { get; }
		EntityBase this[int index] { get; }
		void Add(EntityBase entity);
		void Remove(EntityBase entity);
		void RemoveAt(int index);
		void Replace(EntityBase entity);
		void Replace(int indexToReplace, EntityBase newEntity);
    }
}
