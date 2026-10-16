using Pea.Fitness;
using Pea.Population;

namespace Pea.StopCriteria
{
    public interface IStopCriteria
    {
        StopDecision MakeDecision(IPopulation population, IFitnessComparer fitnessComparer);
    }
}
