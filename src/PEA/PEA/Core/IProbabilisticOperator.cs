using System.Collections.Generic;

namespace Pea.Core
{
    public interface IProbabilisticOperator : IOperator
    {
        void Learn(IList<IChromosome> chromosomes);
        IList<IChromosome> GetSamples();
    }

    public interface IProbabilisticOperator<TC> : IProbabilisticOperator where TC : IChromosome
    {
        new void Learn(IList<IChromosome> parents);
        new IList<IChromosome> GetSamples();
    }
}
