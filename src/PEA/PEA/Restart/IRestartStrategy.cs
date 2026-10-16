using Pea.Entity;
using Pea.Population;

namespace Pea.Restart
{
    public interface IRestartStrategy
    {
        bool ShouldRestart(int iteration, IPopulation population);

        EntityList GetRemainingEntities(IPopulation population);
    }
}
