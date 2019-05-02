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
