using System.Collections.Generic;

namespace Pea.Core
{
    public interface IMigrationStrategy
    {
        IList<IEntity> SelectForTraveling(IList<IEntity> population);
        bool TravelerReceptionDecision(IList<IEntity> population);
        IList<IEntity> InsertMigrants(IList<IEntity> population, IList<IEntity> travelers);
    }
}
