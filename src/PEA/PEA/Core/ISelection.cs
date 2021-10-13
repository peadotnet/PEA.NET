namespace Pea.Core
{
	public interface ISelection
    {
        IEntityList Select(IEntityList entities, int count);
    }
}
