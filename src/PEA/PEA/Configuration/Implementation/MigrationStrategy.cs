using System;
using System.Collections.Generic;

namespace Pea.Configuration.Implementation
{
    public class MigrationStrategy
    {
        public Type StrategyType { get; set; }

        public MigrationStrategy(Type strategyType)
        {
            StrategyType = strategyType;
        }

        public List<IBuildAction> Selections = new List<IBuildAction>();

        public List<IBuildAction> Reinsertions = new List<IBuildAction>();

    }
}
