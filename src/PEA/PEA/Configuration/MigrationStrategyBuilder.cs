using System;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Configuration
{
    public class MigrationStrategyBuilder : PeaSettingsBuilder
    {
        public MigrationStrategy MigrationStrategy { get; set; }

        public MigrationStrategyBuilder(Type strategyType)
        {
            MigrationStrategy = new MigrationStrategy(strategyType);
        }

        public MigrationStrategyBuilder WithSelection<ST>() where ST : ISelection
        {
            MigrationStrategy.SelectionType = typeof(ST);
            return this;
        }

        public MigrationStrategyBuilder WithReinsertion<TR>() where TR : IReplacement
        {
            MigrationStrategy.ReinsertionType = typeof(TR);
            return this;
        }
    }
}
