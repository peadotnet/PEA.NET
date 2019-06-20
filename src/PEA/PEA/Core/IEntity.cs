using System;

namespace Pea.Core
{
    public interface IEntity : ICloneable
    {
        IGenotype Genotype { get; set; }

        IPhenotype Phenotype { get; set; }

        IFitness Fitness { get; set; }
    }
}

