using Pea.Entity;

namespace Pea.Core
{
    public interface IRestartStrategy
    {
        bool ShouldRestart(int iteration, IPopulation population);

        EntityList GetRemainingEntities(IPopulation population);
    }
}
