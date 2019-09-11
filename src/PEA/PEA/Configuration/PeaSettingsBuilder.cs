using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Configuration
{
    public class PeaSettingsBuilder
    {
        protected PeaSettings PeaSettings = new PeaSettings();
        public List<SubProblemBuilder> SubProblemBuilders = new List<SubProblemBuilder>();

        public SubProblemBuilder AddSubProblem()
        {
            var problemBuilder = new SubProblemBuilder();
            SubProblemBuilders.Add(problemBuilder);
            return problemBuilder;
        }

        public PeaSettings Build()
        {
            foreach (var subProblem in SubProblemBuilders)
            {
                PeaSettings.SubProblemList.Add(subProblem.Build());
            }

            return PeaSettings;
        }
    }
}
