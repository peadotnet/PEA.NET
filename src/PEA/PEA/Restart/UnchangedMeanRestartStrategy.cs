using Pea.Core;
using Pea.Util.Statistics;
using System;

namespace Pea.Restart
{
    public class UnchangedMeanRestartStrategy : IRestartStategy
    {
        public double Tolerance { get; set; }  = 100;
        public int IterationWindow { get; set; }  = 1000;
        public int MaxNumberOfRestarts { get; set; } = 10;


        int LastChangedIteration = 0;

        IStatisticsArray LastVariances;

        public UnchangedMeanRestartStrategy()
        {
            
        }


        public EntityList GetRemainingEntities(IPopulation population)
        {
            var entities = new EntityList(population.MaxNumberOfEntities);
            entities.AddRange(population.Bests);
            return entities;
        }

        public bool ShouldRestart(int iteration, IPopulation population)
        {
            if (MaxNumberOfRestarts < 0)
            {
                return false;
            }

            var variances = population.FitnessStatistics;

            if (LastVariances != null)
            {
                if ((iteration - LastChangedIteration) < IterationWindow)
                {
                    return false;
                }
            }
            else
            {
                LastVariances = new StatisticsArray(variances.Length);
                StatisticsArray.CopyTo(variances, LastVariances);
                LastChangedIteration = iteration;
                return false;
            }

            LastChangedIteration = iteration;

            bool changed = false;

            for (int i = 0; i < variances.Length; i++)
            {
                if (Math.Abs(LastVariances[i].Mean - variances[i].Mean) > Tolerance)
                {
                    changed = true;
                    break;
                }
            }

            for (int i = 0; i < variances.Length; i++)
            {
                variances[i].CopyTo(LastVariances[i]);
            }

            if (!changed)
            {
                MaxNumberOfRestarts--;
            }

            return !changed;
        }
    }
}
