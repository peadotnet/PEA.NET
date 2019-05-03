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
                bests.Add(entity);
            }

            return bests;
        }

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
