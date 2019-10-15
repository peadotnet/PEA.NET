using System.Collections.Generic;
using Pea.Core;

namespace Pea.Migration.Implementation
{
    public abstract class MigrationStrategyBase : IMigrationStrategy
    {
        public IRandom Random { get; }
        public ParameterSet Parameters { get; }
        public ISelection Selection { get; }
        public IReplacement Reinsertion { get; }
 
        protected MigrationStrategyBase(IRandom random, ISelection selection, IReplacement reinsertion, ParameterSet parameters)
        {
            Random = random;
            Selection = selection;
            Reinsertion = reinsertion;
            Parameters = parameters;
        }

        public abstract IList<IEntity> SelectForTraveling(IList<IEntity> population);

        public abstract bool TravelerReceptionDecision(IList<IEntity> population);

        public abstract IList<IEntity> InsertMigrants(IList<IEntity> population, IList<IEntity> travelers);
    }
}
