using System.Collections;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer : IComparer, IComparer<IFitness>
    {
        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        bool Dominates(object x, object y);

        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        bool Dominates(IFitness x, IFitness y);

        bool MergeToBests(IList<IEntity> bests, IEntity entity);
    }
}
