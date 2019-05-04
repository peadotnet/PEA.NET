using System.Collections.Generic;

namespace Pea.Core
{
    public interface IGenotype
    {
        IDictionary<string, IChromosome> Chromosomes { get; }
    }
}
