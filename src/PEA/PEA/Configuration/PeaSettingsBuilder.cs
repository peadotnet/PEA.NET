using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.StopCriteria;

namespace Pea.Configuration
{
    public class PeaSettingsBuilder
    {
        protected PeaSettings PeaSettings = new PeaSettings();
        protected List<SubProblemBuilder> SubProblems = new List<SubProblemBuilder>();
        protected StopCriteriaBuilder StopCriteria;

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
            PeaSettings.ParameterSet.Add(new PeaSettingsNamedValue(parameterKey, parameterValue));
            return this;
        }

        public PeaSettings Build()
        {
            foreach (var subProblem in SubProblems)
            {
                PeaSettings.SubProblemList.Add(subProblem.Build());
            }

            return PeaSettings;
        }
    }
}
