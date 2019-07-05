using Pea.Core;

namespace Pea.Fitness.Implementation.MultiObjective
{
    public class MultiObjectiveFitness : IFitness<double[]>
    {
        public double[] Value { get; set; }
        public bool IsValid { get; set; }

        public MultiObjectiveFitness(int numberOfObjectives)
        {
            Value = new double[numberOfObjectives];
        }

        public bool IsEquivalent(IFitness other)
        {
            var otherFitness = other as MultiObjectiveFitness;
            if (otherFitness == null) return false;

            if (Value.Length != otherFitness.Value.Length) return false;
            for (int i = 0; i < Value.Length; i++)
            {
                var difference = Value[i] - otherFitness.Value[i];
                if (difference > double.Epsilon || difference < -1 * double.Epsilon) return false;
            }

            return true;
        }
    }
}
