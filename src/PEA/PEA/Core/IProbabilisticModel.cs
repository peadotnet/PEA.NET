using System.Collections.Generic;

namespace Pea.Core
{
    public interface IProbabilisticModel : IOperator
    {
        void Learn(IList<IChromosome> chromosomes);
        void Add(IChromosome chromosome);
        void Remove(IChromosome chromosome);

        IChromosome GetSample();
    }

    public interface IProbabilisticModel<TC> : IProbabilisticModel where TC : IChromosome
    {
        new void Learn(IList<IChromosome> chromosomes);
        new void Add(IChromosome chromosome);
        new void Remove(IChromosome chromosome);

        new IChromosome GetSample();
    }
}
