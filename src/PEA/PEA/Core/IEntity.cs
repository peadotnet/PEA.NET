namespace Pea.Core
{
    public interface IEntity
    {
        IGenotype Genotype { get; set; }

        IPhenotype Phenotype { get; set; }

        IFitness Fitness { get; set; }
    }
}

