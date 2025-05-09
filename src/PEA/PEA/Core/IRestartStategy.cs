namespace Pea.Core
{
    public interface IRestartStategy
    {
        bool ShouldRestart(int iteration, IPopulation population);

        EntityList GetRemainingEntities(IPopulation population);
    }
}
