using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Configuration.Implementation
{
    public class PeaSettings
    {
        public List<SubProblem> SubProblemList = new List<SubProblem>();

        public List<PeaSettingsNamedValue> ParameterSet = new List<PeaSettingsNamedValue>();

        public MigrationStrategy MigrationStrategy { get; set; }

        public Type FitnessEvaluation { get; set; }

        public Type Random { get; set; } = typeof(FastRandom);

        public IStopCriteria StopCriteria { get; set; }

        public PeaSettings GetIslandSettings(MultiKey key)
        {
            var islandSettings = new PeaSettings()
            {
                MigrationStrategy = this.MigrationStrategy,
                FitnessEvaluation = this.FitnessEvaluation,
                StopCriteria = this.StopCriteria
            };

            foreach (var subProblem in SubProblemList)
            {
                if (key.Contains(subProblem.Encoding.Key))
                {
                    islandSettings.SubProblemList.Add(subProblem);
                }
            }

            return islandSettings;
        }

    }
}
