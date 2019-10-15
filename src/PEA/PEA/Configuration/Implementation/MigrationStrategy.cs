using System;

namespace Pea.Configuration.Implementation
{
    public class MigrationStrategy
    {
        public Type StrategyType { get; set; }

        public Type SelectionType { get; set; }

        public Type ReinsertionType { get; set; }

        public MigrationStrategy(Type strategyType)
        {
            StrategyType = strategyType;
        }
    }
}
