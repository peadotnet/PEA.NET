using Pea.Core;
using System;
using System.Diagnostics;

namespace Pea.StopCriteria.Implementation
{
    public class TimeOutStopCriteria : IStopCriteria
    {
        public int TimeoutMilliseconds { get; }
        public Stopwatch StopWatch { get; private set; } = null;

        public TimeOutStopCriteria()
        {
            TimeoutMilliseconds = 600000;
        }

        public TimeOutStopCriteria(int timeoutMilliseconds)
        {
            TimeoutMilliseconds = timeoutMilliseconds;
        }

        public StopDecision MakeDecision(IEngine engine, IPopulation population)
        {
            if (StopWatch == null)
            {
                StopWatch = Stopwatch.StartNew();
                return new StopDecision(false);
            }

            if (StopWatch.ElapsedMilliseconds < TimeoutMilliseconds)
            {
                return new StopDecision(false);
            }

            StopWatch.Stop();
            return new StopDecision(true, "Timeout expired");
        }
    }
}
