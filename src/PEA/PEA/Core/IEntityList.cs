namespace Pea.Core
{
	public interface IEntityList
	{
		int Count { get; }
		IEntity this[int index] { get; }
		void Add(IEntity entity);
		void Remove(IEntity entity);
		void RemoveAt(int index);
		void Replace(IEntity entity);
	}
}
