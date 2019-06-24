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

    public interface IEntityCreator<TG1, TG2> : IEntityCreator<TG1>
        where TG1: IGenotype where TG2 : IGenotype
    {

    }


}
