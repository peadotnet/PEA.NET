using System.Collections.Generic;
using Pea.Core;

namespace Pea.Migration.Implementation
{
    public abstract class MigrationStrategyBase : IMigrationStrategy
    {
        public IRandom Random { get; protected set; }
        public ParameterSet Parameters { get; protected set; }
        public ISelection Selection { get; protected set; }
        public IReplacement Reinsertion { get; protected set; }
 
        protected MigrationStrategyBase(IRandom random, ISelection selection, IReplacement reinsertion, ParameterSet parameters)
        {
            Random = random;
            Selection = selection;
            Reinsertion = reinsertion;
            Parameters = parameters;
        }

        public abstract IEntityList SelectForTraveling(IPopulation population);

        public abstract bool TravelerReceptionDecision(IPopulation population);

        public abstract IEntityList InsertMigrants(IPopulation population, IEntityList travelers);
    }
}
