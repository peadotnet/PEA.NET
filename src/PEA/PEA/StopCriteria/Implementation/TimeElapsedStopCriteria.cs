using Pea.Core;
using System;
using System.Diagnostics;

namespace Pea.StopCriteria.Implementation
{
    public class TimeOutStopCriteria : IStopCriteria
    {
        public int TimeoutSeconds { get; }
        public Stopwatch StopWatch { get; private set; } = null;

        public TimeOutStopCriteria()
        {
            TimeoutSeconds = 600;
        }

        public TimeOutStopCriteria(int timeoutSeconds)
        {
            TimeoutSeconds = timeoutSeconds;
        }

        public StopDecision MakeDecision(IEngine engine, IPopulation population)
        {
            if (StopWatch == null)
            {
                StopWatch = Stopwatch.StartNew();
                return new StopDecision(false);
            }

            if (StopWatch.ElapsedMilliseconds > 1000 * TimeoutSeconds)
            {
                return new StopDecision(false);
            }

            StopWatch.Stop();
            return new StopDecision(true, "Timeout expired");
        }
    }
}
