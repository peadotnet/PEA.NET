using Pea.Core;

namespace Pea.StopCriteria.Implementation
{
    public class OrStopCriteria : IStopCriteria
    {
        public IStopCriteria Criteria1 { get; }
        public IStopCriteria Criteria2 { get; }

        public OrStopCriteria(IStopCriteria criteria1, IStopCriteria criteria2)
        {
            Criteria1 = criteria1;
            Criteria2 = criteria2;
        }

        public StopDecision MakeDecision(IEngine engine, IPopulation population)
        {
            var decision1 = Criteria1.MakeDecision(engine, population);
            var decision2 = Criteria2.MakeDecision(engine, population);

            var decision = new StopDecision(decision1.MustStop || decision2.MustStop);
            decision.Reasons.AddRange(decision1.Reasons);
            decision.Reasons.AddRange(decision2.Reasons);

            return decision;
        }
    }
}
