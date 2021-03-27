using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.StopCriteria;

namespace Pea.Configuration
{
    public class PeaSettingsBuilder
    {
        protected PeaSettings Settings = new PeaSettings();
        protected List<SubProblemBuilder> SubProblems = new List<SubProblemBuilder>();
        protected StopCriteriaBuilder StopCriteria;
        protected MigrationStrategyBuilder MigrationStrategy;

        public SubProblemBuilder AddSubProblem()
        {
            var problemBuilder = new SubProblemBuilder();
            SubProblems.Add(problemBuilder);
            return problemBuilder;
        }

        public SubProblemBuilder AddSubProblem(string key, IProblemModel problemModel)
        {
            var problemBuilder = problemModel.Apply(key, new SubProblemBuilder());
            SubProblems.Add(problemBuilder);
            return problemBuilder;
        }

        public StopCriteriaBuilder StopWhen()
        {
            StopCriteria = StopCriteriaBuilder.StopWhen();
            return StopCriteria;
        }

        public PeaSettingsBuilder SetParameter(string parameterKey, double parameterValue)
        {
            Settings.ParameterSet.Add(new PeaSettingsNamedValue(parameterKey, parameterValue));
            return this;
        }

        public PeaSettingsBuilder WithEvaluation<TE>() where TE : IEvaluation
        {
            Settings.Evaluation = typeof(TE);
            return this;
        }

        public PeaSettingsBuilder WithEntityType<NE>() where NE : IEntity
        {
            Settings.EntityType = typeof(NE);
            return this;
        }

        public MigrationStrategyBuilder WithMigrationStrategy<TM>() where TM : IMigrationStrategy
        {
            MigrationStrategy = new MigrationStrategyBuilder(typeof(TM));
            return MigrationStrategy;
        }

        public PeaSettingsBuilder WithRandom<TR>() where TR : IRandom
		{
            Settings.Random = typeof(TR);
            return this;
		}

        public PeaSettingsBuilder WithSeed(int seed)
		{
            Settings.Seed = seed;
            return this;
		}

        public PeaSettings Build()
        {
            foreach (var subProblem in SubProblems)
            {
                Settings.SubProblemList.Add(subProblem.Build());
            }

            Settings.StopCriteria = StopCriteria.Build();

            return Settings;
        }
    }
}
