using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Configuration.Implementation
{
    public class PeaSettings
    {
        public List<SubProblem> SubProblemList = new List<SubProblem>();

        public MigrationStrategy MigrationStrategy { get; set; }

        public Type FitnessEvaluation { get; set; }

        public IStopCriteria StopCriteria { get; set; }
    }
}
