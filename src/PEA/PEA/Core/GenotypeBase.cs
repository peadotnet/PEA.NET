using System.Collections.Generic;

namespace Pea.Core
{
    public abstract class GenotypeBase : IGenotype
    {
        public IDictionary<string, IChromosome> Chromosomes { get; }

        public GenotypeBase(Dictionary<string, IChromosome> chromosomes)
        {
            Chromosomes = chromosomes;
        }

        public void AddChromosome(string name, IChromosome chromosome)
        {
            Chromosomes.Add(name, chromosome);
        }

    }
}


