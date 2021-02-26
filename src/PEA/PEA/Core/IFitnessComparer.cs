using System.Collections;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer : IComparer
    {
        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        bool Dominates(object x, object y);

        bool MergeToBests(IList<IEntity> bests, IEntity entity);
    }

    public interface IFitnessComparer<TF> : IFitnessComparer, IComparer<IFitness<TF>>
    {
        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        bool Dominates(IFitness<TF> x, IFitness<TF> y);
    }
}
