using System.Collections.Generic;

namespace Pea.Core
{
    public interface IChromosomeFactory
    {
        IEnumerable<IMutation> GetMutations();
        IEnumerable<ICrossover> GetCrossovers();
    }

    public interface IChromosomeFactory<TC> : IChromosomeFactory where TC: IChromosome
    {
        IChromosomeFactory<TC> AddMutations(IEnumerable<IMutation<TC>> mutations);
        IChromosomeFactory<TC> AddCrossovers(IEnumerable<ICrossover<TC>> crossovers);
    }
}
