using Pea.Entity;
using Pea.Util.Statistics;
using System.Collections.Generic;

namespace Pea.Engine
{
    public interface IEvolutionStateReportData
    {
        int Iteration { get; set; }
        IList<EntityBase> BestEntities { get; set; }
        IStatisticsArray FitnessStatistics { get; set; }
    }
}
