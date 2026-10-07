using Pea.Core;
using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Fitness.Implementation.MultiObjective
{
    public class ParetoComparerWithConstraintViolationReduction : IFitnessComparer
    {
        /// <inheritdoc/>
        public int Compare(IFitness x, IFitness y)
        {
            if (ConstraintsAreViolated(x, y)) return CompareConstraintViolations(x, y);

            if (Dominates(x, y)) return 1;
            if (Dominates(y, x)) return -1;
            return 0;
        }

        private bool ConstraintsAreViolated(IFitness x, IFitness y)
        {
            return x.ConstraintViolation > 0 || y.ConstraintViolation > 0;
        }

        private int CompareConstraintViolations(IFitness x, IFitness y)
        {
            if (x.ConstraintViolation > 0 && y.ConstraintViolation > 0)
            {
                var comparison = x.ConstraintViolation.CompareTo(y.ConstraintViolation);
                return comparison;
            }

            if (x.ConstraintViolation > 0) return 1;
            return -1;
        }

        /// <inheritdoc/>
        public bool MergeToBests(IList<EntityBase> bests, EntityBase entity)
        {
            bool hasToBeAdded = true;

            for (int i = bests.Count - 1; i >= 0; i--)
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
                bests.Add(entity);
            }

            return hasToBeAdded;
        }

        /// <inheritdoc/>
        public bool Dominates(IFitness x, IFitness y)
        {
            var dominates = false;

            for (int i = 0; i < x.Value.Count; i++)
            {
                var diff = x.Value[i] - y.Value[i];

                if (diff > double.Epsilon)
                {
                    return false;
                }

                if (diff < -1 * double.Epsilon)
                {
                    dominates = true;
                }
            }

            return dominates;
        }
    }
}
