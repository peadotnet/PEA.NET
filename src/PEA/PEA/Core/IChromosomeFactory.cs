using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public interface IChromosomeFactory : IEngineModifier
    {
        IEnumerable<PeaSettingsNamedValue> GetParameters();
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
