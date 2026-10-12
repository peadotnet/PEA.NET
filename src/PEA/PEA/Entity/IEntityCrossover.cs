namespace Pea.Entity
{
	public interface IEntityCrossover
    {
        IEntityList Cross(IEntityList parents, int count);
    }
}
