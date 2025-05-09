using Pea.Util.Statistics;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface IPopulation : IEntityList
    {
        IList<IEntity> Bests { get; set; }
        int MaxNumberOfEntities { get; set; }
        int MinNumberOfEntities { get; set; }
        IStatisticsArray FitnessStatistics { get; }
        void AddRange(EntityList entityList);

        void Sort(IComparer<IEntity> comparer);
        IPopulation CloneEmpty();
    }
}
