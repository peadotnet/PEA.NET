using System.Collections.Generic;

namespace Pea.Core
{
    public interface ICrossover : IGeneticOperator
    {
        IList<IChromosome> Cross(IChromosome parent0, IChromosome parent1);
    }

    public interface ICrossover<TC> : ICrossover where TC: IChromosome
    {
        new IList<IChromosome> Cross(IChromosome parent0, IChromosome parent1);
    }
}
