using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Core
{
    interface IStopCriteria
    {
        StopDecision MakeDecision(IEngine engine, IPopulation population);
    }
}
