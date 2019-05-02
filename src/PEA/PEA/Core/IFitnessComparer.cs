using System.Collections;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IFitnessComparer : IComparer
    {

    }

    public interface IFitnessComparer<TF> : IFitnessComparer, IComparer<IFitness<TF>>
    {

    }
}
