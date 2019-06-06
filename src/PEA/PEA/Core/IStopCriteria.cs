using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Core
{
    public interface IStopCriteria
    {
        StopDecision MakeDecision(IEngine engine, IPopulation population);
    }
}
