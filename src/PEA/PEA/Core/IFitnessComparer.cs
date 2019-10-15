using System.Collections;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer : IComparer
    {
        bool Dominates(object x, object y);

        bool MergeToBests(IList<IEntity> bests, IEntity entity);
    }

    public interface IFitnessComparer<TF> : IFitnessComparer, IComparer<IFitness<TF>>
    {
        bool Dominates(IFitness<TF> x, IFitness<TF> y);
    }
}
