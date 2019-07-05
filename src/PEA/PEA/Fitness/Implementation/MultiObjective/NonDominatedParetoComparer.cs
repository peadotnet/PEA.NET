using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Fitness.Implementation.MultiObjective
{
    public class NonDominatedParetoComparer : IFitnessComparer<double[]>
    {
        public int Compare(object x, object y)
        {
            return Compare(x as IFitness<double[]>, y as IFitness<double[]>);
        }

        /// <summary>
        /// Compare two multiobjective fitness value nondominated pareto way
        /// </summary>
        /// <returns>1 if y dominates x, -1 if x dominates y, 0 otherwise</returns>
        public int Compare(IFitness<double[]> x, IFitness<double[]> y)
        {
            if (Dominates(x, y)) return 1;
            if (Dominates(y, x)) return -1;
            return 0;
        }

        public IList<IEntity> MergeToBests(IList<IEntity> bests, IEntity entity)
        {
            bool hasToBeAdded = true;

            for (int i = bests.Count-1; i >= 0; i--)
            {
                if (bests[i].Fitness.IsEquivalent(entity.Fitness))
                {
                    hasToBeAdded = false;
                    break;
                }

                switch (Compare(bests[i].Fitness, entity.Fitness))
                {
                    case -1:
                        hasToBeAdded = false;
                        break;
                    case 1:
                        bests.RemoveAt(i);
                        break;
                }
            }

            if (hasToBeAdded)
            {
                var timeString = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "." +
                                 DateTime.Now.Millisecond;
                Console.WriteLine(timeString + " " + entity.ToString());
                bests.Add(entity);
            }

            return bests;
        }

        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if y dominates x, false otherwise</returns>
        private bool Dominates(IFitness<double[]> x, IFitness<double[]> y)
        {
            var dominates = false;
            for (int i = 0; i < x.Value.Length; i++)
            {
                var diff = x.Value[i] - y.Value[i];
                if (diff > double.Epsilon)
                {
                    return false;
                }
                else if (diff < -1 * double.Epsilon)
                {
                    dominates = true;
                }
            }

            return dominates;
        }
    }
}
