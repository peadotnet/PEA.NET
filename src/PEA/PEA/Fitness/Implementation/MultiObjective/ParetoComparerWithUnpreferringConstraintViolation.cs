﻿using System.Collections.Generic;
using Pea.Core;

namespace Pea.Fitness.Implementation.MultiObjective
{
	public class ParetoComparerWithUnpreferringConstraintViolation : IFitnessComparer
    {
        public int Compare(object x, object y)
        {
            return Compare(x as IFitness, y as IFitness);
        }

        /// <summary>
        /// Compare two multiobjective fitness value nondominated pareto way
        /// </summary>
        /// <returns>1 if y dominates x, -1 if x dominates y, 0 otherwise</returns>
        public int Compare(IFitness x, IFitness y)
        {
            if (x.ConstraintViolation > 0 && y.ConstraintViolation <= 0) return 1;
            if (x.ConstraintViolation <= 0 && y.ConstraintViolation > 0) return -1;

			if (Dominates(x, y)) return 1;
            if (Dominates(y, x)) return -1;

            return 0;
        }

		public int CompareConstraintViolations(IFitness x, IFitness y)
		{
            if (x.ConstraintViolation > 0 && y.ConstraintViolation > 0)
            {
                var comparison = x.ConstraintViolation.CompareTo(y.ConstraintViolation);
                return comparison;
            }

			if (x.ConstraintViolation > 0) return 1;
            return -1;
		}

		public bool MergeToBests(IList<IEntity> bests, IEntity entity)
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
                bests.Add(entity);
            }

            return hasToBeAdded;
        }

        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if the second (y) dominates the first (x), false otherwise</returns>
        public bool Dominates(object x, object y)
        {
            return Dominates(x as IFitness, y as IFitness);
        }

        /// <summary>
        /// Indicates whether the multiobjective fitness y dominates x
        /// </summary>
        /// <returns>True if y dominates x, false otherwise</returns>
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
