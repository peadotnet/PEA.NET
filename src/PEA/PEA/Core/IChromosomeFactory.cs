using System.Collections.Generic;

namespace Pea.Core
{
    public interface IChromosomeFactory<TC> where TC: IChromosome
    {
        IEnumerable<IMutation<TC>> GetMutations();
        IEnumerable<ICrossover<TC>> GetCrossovers();
        IChromosomeFactory<TC> AddMutations(IEnumerable<IMutation<TC>> mutations);
        IChromosomeFactory<TC> AddCrossovers(IEnumerable<ICrossover<TC>> crossovers);
    }
}
