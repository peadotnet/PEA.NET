using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer
    {
        /// <summary>
        /// Compare two fitness value nondominated pareto way
        /// </summary>
        /// <returns>1 if y is strictly better than x, -1 if x strictly better than y, 0 otherwise (equal or non-dominated)</returns>
        int Compare(IFitness x, IFitness y);

        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        bool Dominates(IFitness x, IFitness y);

        bool MergeToBests(IList<EntityBase> bests, EntityBase entity);
    }
}
