using System.Collections;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer : IComparer
    {
        IList<IEntity> MergeToBests(IList<IEntity> bests, IEntity entity);
    }

    public interface IFitnessComparer<TF> : IFitnessComparer, IComparer<IFitness<TF>>
    {

    }
}
