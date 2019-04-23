namespace Pea.Core
{
    public interface IEntityCreator<TG> where TG: IGenotype
    {
        IEntity<TG> Create();
    }
}
