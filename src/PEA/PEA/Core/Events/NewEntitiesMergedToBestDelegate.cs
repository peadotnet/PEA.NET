using Pea.Core.Entity;
using Pea.Util.Statistics;
using System;
using System.Collections.Generic;

namespace Pea.Core.Events
{
    public delegate void NewEntitiesMergedToBestDelegate(NewEntitiesMergedToBestEventArgs e);

    public class NewEntitiesMergedToBestEventArgs : EventArgs, IEvolutionStateReportData
    {    
        public int Iteration { get; set; }
        public IList<EntityBase> BestEntities { get; set; }
        public IStatisticsArray FitnessStatistics { get; set; }
    }
}
