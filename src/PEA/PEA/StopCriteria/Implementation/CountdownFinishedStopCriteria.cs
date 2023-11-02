using Pea.Core;

namespace Pea.StopCriteria.Implementation
{
    public class CountdownFinishedStopCriteria : IStopCriteria
    {
        public int Counter { get; set; }

        public CountdownFinishedStopCriteria()
        {
            Counter = 60 * 1000 * 100;
        }

        public CountdownFinishedStopCriteria(int counter) 
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
