using Pea.Core;

namespace Pea.StopCriteria.Implementation
{
    public class IterationsRechedStopCriteria : IStopCriteria
    {
        public int Counter { get; set; }

        public IterationsRechedStopCriteria()
        {
            Counter = 60 * 1000 * 100;
        }

        public IterationsRechedStopCriteria(int counter) 
        {
            Counter = counter;
        }

        public StopDecision MakeDecision(IEngine engine, IPopulation population)
        {
            Counter--;
            if (Counter < 1) return new StopDecision(true, "Countdown is finished.");
            return new StopDecision(false);
        }
    }
}
