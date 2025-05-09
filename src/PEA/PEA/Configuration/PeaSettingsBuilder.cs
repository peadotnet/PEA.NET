using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Core.Events;
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

        public SubProblemBuilder AddSubProblem(string key, IScenario problemModel)
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

        public PeaSettingsBuilder WithRestartStrategy(IRestartStategy restartStategy)
        {
            Settings.RestartStategy = restartStategy;
            return this;
        }

        public PeaSettingsBuilder SetParameter(string parameterKey, double parameterValue)
        {
            Settings.ParameterSet.Add(new PeaSettingsNamedValue(parameterKey, parameterValue));
            return this;
        }

        public PeaSettingsBuilder WithEvaluation<TE>() where TE : EvaluationBase
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

        public PeaSettingsBuilder AddBestMergedCallback(NewEntitiesMergedToBestDelegate callback)
		{
            Settings.NewEntityMergedToBest.Add(callback);
            return this;
		}

        public PeaSettings Build()
        {
            Settings.SubProblemList.Clear();
            foreach (var subProblem in SubProblems)
            {
                Settings.SubProblemList.Add(subProblem.Build());
            }

            Settings.StopCriteria = StopCriteria.Build();

            return Settings;
        }
    }
}
