namespace Pea.Core
{
    public interface IEntity
    {

    }

    public interface IEntity<TG> : IEntity where TG: IGenotype
    {
        TG Genotype { get; set; }
    }
}

