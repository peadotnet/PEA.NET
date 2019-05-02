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
    }
}
