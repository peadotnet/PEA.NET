using System.Collections.Generic;

namespace Pea.Core
{
    public interface ICrossover
    {
        IList<IChromosome> Cross(IList<IChromosome> parents);
    }

    public interface ICrossover<TC> : ICrossover where TC: IChromosome
    {
        new IList<IChromosome> Cross(IList<IChromosome> parents);
    }
}
