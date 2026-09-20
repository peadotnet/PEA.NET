using Pea.Core.Entity;
using Pea.Util.Statistics;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IEvolutionStateReportData
    {
        int Iteration { get; set; }
        IList<EntityBase> BestEntities { get; set; }
        IStatisticsArray FitnessStatistics { get; set; }
    }
}
