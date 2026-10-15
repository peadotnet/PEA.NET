using Pea.Fitness;
using Pea.Population;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Core
{
    public interface IStopCriteria
    {
        StopDecision MakeDecision(IPopulation population, IFitnessComparer fitnessComparer);
    }
}
