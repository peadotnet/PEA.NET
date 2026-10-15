using Pea.Entity;

namespace Pea.Population
{
	public interface ISelection
    {
        IEntityList Select(IEntityList entities, int count);
    }
}
