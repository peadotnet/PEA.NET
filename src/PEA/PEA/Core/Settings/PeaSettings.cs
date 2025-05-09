using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core.Events;
using Pea.Core.Settings;

namespace Pea.Core
{
    [Obsolete("This class is obsolete. Use Pea.Configuration.Implementation.PeaSettings instead.")]
    public class PeaSettings_old
    {
        public Type Random { get; set; }

        public Type Algorithm { get; set; }

        public IList<PeaSettingsNamedType> Chromosomes { get; set; } =
            new List<PeaSettingsNamedType>();

        public IList<PeaSettingsNamedValue> ParameterSet { get; set; } =
            new List<PeaSettingsNamedValue>();

        public IList<PeaSettingsNamedType> EntityCreators { get; set; } =
            new List<PeaSettingsNamedType>();

        public IList<PeaSettingsTypeProbability> Selectors { get; set; } =
            new List<PeaSettingsTypeProbability>();

        public IList<PeaSettingsTypeProbability> Reinsertions { get; set; } =
            new List<PeaSettingsTypeProbability>();

        public IStopCriteria StopCriteria { get; set; }

        public IRestartStategy RestartStategy { get; set; }

        public Type ConflictDetector { get; set; }
        public Type Fitness { get; set; }
        public Type PhenotypeDecoder { get; set; }
        public Type Evaluation { get; set; }
        public NewEntitiesMergedToBestDelegate NewEntityMergedToBest { get; set; }
    }
}
