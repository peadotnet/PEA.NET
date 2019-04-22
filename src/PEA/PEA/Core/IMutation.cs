namespace Pea.Core
{
    public interface IMutation<TG>
    {
        IEntity<TG> Mutate(IEntity<TG> entity);
    }
}
