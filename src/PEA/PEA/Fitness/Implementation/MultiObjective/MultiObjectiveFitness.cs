using Pea.Core;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Pea.Fitness.Implementation.MultiObjective
{
    public class MultiObjectiveFitness : IFitness<double>
    {
        public IEntity Entity { get; set; }
        public IReadOnlyList<double> Value { get; }
        public bool IsValid { get; set; }

        public MultiObjectiveFitness(params double[] values)
        {
            Value = ImmutableArray.Create<double>(values);
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
