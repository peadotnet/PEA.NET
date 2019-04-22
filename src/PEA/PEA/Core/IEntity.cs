namespace Pea.Core
{
    public interface IEntity
    {

    }

    public interface IEntity<TG> : IEntity
    {
        TG Genotype { get; set; }
    }
}

