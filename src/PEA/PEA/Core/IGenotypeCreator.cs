namespace Pea.Core
{
    public interface IGenotypeCreator<TG>
    {
        IEntity<TG> Create();
    }
}
