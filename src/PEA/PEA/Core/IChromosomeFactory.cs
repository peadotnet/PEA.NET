using System.Collections.Generic;

namespace Pea.Core
{
    public interface IChromosomeFactory : IEngineModifier
    {
        IEnumerable<ICrossover> GetCrossovers();
        IEnumerable<IMutation> GetMutations();
    }

    public interface IChromosomeFactory<TC> : IChromosomeFactory where TC: IChromosome
    {
        IChromosomeFactory<TC> AddMutations(IEnumerable<IMutation<TC>> mutations);
        IChromosomeFactory<TC> AddCrossovers(IEnumerable<ICrossover<TC>> crossovers);
    }
}
