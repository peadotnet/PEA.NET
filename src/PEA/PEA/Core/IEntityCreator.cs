namespace Pea.Core
{
    public interface IEntityCreator
    {
        IEntity CreateEntity();
    }

    public interface IEntityCreator<TG> : IEntityCreator 
        where TG: IGenotype
    {
    }
}
