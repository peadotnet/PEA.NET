using Pea.Core;

namespace Pea.StopCriteria.Implementation
{
    public class FitnessLimitExceededStopCriteria : IStopCriteria
    {
        public IFitness FitnessLimit { get; }

        public FitnessLimitExceededStopCriteria(IFitness fitnessLimit)
        {
            FitnessLimit = fitnessLimit;
        }

        public StopDecision MakeDecision(IPopulation population, IFitnessComparer fitnessComparer)
        {
            foreach (var bestEntity in population.Bests)
            {
                var exceed = fitnessComparer.Compare(FitnessLimit, bestEntity.Fitness);

                if (exceed == 1)
                {
                    return new StopDecision(true, "Fitness limit exceeded");
                }
            }

            return new StopDecision(false);
        }
    }
}
