namespace Pea.Core
{
	public interface IEntityMutation
    {
        IEntityList Mutate(IEntityList entities);
    }
}
