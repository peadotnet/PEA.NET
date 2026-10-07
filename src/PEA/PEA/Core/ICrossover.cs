using System.Collections.Generic;

namespace Pea.Core
{
    public interface ICrossover : IGeneticOperator
    {
        /// <summary>
        /// Creates a set of child chromosomes from the parents. The method must create new instances 
        /// (and leave the parents unchanged)
        /// </summary>
        /// <param name="parent0">Chromosome of the first parent</param>
        /// <param name="parent1">Chromosome of the second parent</param>
        /// <returns>Children chromosomes: their number can range from zero to any</returns>
        IList<IChromosome> Cross(IChromosome parent0, IChromosome parent1);
    }

    public interface ICrossover<TC> : ICrossover where TC: IChromosome
    {
        new IList<IChromosome> Cross(IChromosome parent0, IChromosome parent1);
    }
}
