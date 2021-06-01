using Pea.Core;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Pea.Fitness.Implementation.MultiObjective
{
    public class MultiObjectiveFitness : IFitness<double>
    {
        public IEntity Entity { get; set; }
        public IReadOnlyList<double> Value { get; }
        public double ConstraintViolation { get; }
        public bool IsValid { get; set; }

        public int TournamentWinner { get; set; } = 0;
        public int TournamentLoser { get; set; } = 0;

        public int DominationCount { get; set; }
        public IList<int> DominatedEnities { get; set; }

        public int Rank { get; set; }
        public double CrowdingDistance { get; set; }

        public MultiObjectiveFitness(double[] values, double constraintViolation = 0)
        {
            Value = ImmutableArray.Create<double>(values);
            ConstraintViolation = constraintViolation;
        }

        public bool IsEquivalent(IFitness other)
        {
            var otherFitness = other as MultiObjectiveFitness;
            if (otherFitness == null) return false;

            if (Value.Count != otherFitness.Value.Count) return false;
            for (int i = 0; i < Value.Count; i++)
            {
                var difference = Value[i] - otherFitness.Value[i];
                if (difference > double.Epsilon || difference < -1 * double.Epsilon) return false;
            }

            return true;
        }

		public bool IsLethal()
		{
            for(int i=0; i< Value.Count; i++)
			{
                if (double.IsInfinity(Value[i])) return true;
			}
            return false;
		}
	}
}
