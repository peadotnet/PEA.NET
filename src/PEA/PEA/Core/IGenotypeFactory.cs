using System.Collections.Generic;

namespace Pea.Core
{
    public interface IGenotypeFactory<TG>
    {
        IEnumerable<IGenotypeCreator<TG>> GetCreators();
        IEnumerable<IMutation<TG>> GetMutations();
        IEnumerable<ICrossover<TG>> GetCrossovers();
        IGenotypeFactory<TG> AddCreators(IEnumerable<IGenotypeCreator<TG>> creators);
        IGenotypeFactory<TG> AddMutations(IEnumerable<IMutation<TG>> mutations);
        IGenotypeFactory<TG> AddCrossovers(IEnumerable<ICrossover<TG>> crossovers);
    }
}
