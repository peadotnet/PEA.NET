using System.Collections.Generic;

namespace Pea.Core
{
    public interface ICrossover<TC> where TC: IChromosome
    {
        IList<TC> Cross(IList<TC> parents);
    }
}
