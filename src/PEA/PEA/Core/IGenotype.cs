using System.Collections.Generic;

namespace Pea.Core
{
    public interface IGenotype
    {
        IEnumerable<IChromosome> Chromosomes { get; }
    }
}
