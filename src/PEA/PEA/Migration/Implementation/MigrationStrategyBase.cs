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

        public abstract IEntityList SelectForTraveling(IPopulation population);

        public abstract bool TravelerReceptionDecision(IPopulation population);

        public abstract IEntityList InsertMigrants(IPopulation population, IEntityList travelers);
    }
}
