namespace Pea.Core
{
	public interface IMigrationStrategy
    {
        IEntityList SelectForTraveling(IPopulation population);
        bool TravelerReceptionDecision(IPopulation population);
        IEntityList InsertMigrants(IPopulation population, IEntityList travelers);
    }
}
