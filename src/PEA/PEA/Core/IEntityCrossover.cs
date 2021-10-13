namespace Pea.Core
{
	public interface IEntityCrossover
    {
        IEntityList Cross(IEntityList parents, int count);
    }
}
