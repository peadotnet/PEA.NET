using System.Collections.Generic;

namespace Pea.Core
{
    public interface IChromosomeFactory : IEngineModifier
    {
        IList<IChromosomeCreator> GetCreators();
        IList<ICrossover> GetCrossovers();
        IList<IMutation> GetMutations();
    }

    public interface IChromosomeFactory<TC> : IChromosomeFactory where TC: IChromosome
    {
        IChromosomeFactory<TC> AddCreators(IEnumerable<IChromosomeCreator<TC>> creators);
        IChromosomeFactory<TC> AddMutations(IEnumerable<IMutation<TC>> mutations);
        IChromosomeFactory<TC> AddCrossovers(IEnumerable<ICrossover<TC>> crossovers);
    }
}
