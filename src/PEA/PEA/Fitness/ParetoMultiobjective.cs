using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;

namespace Pea.Fitness
{
    public class ParetoMultiobjective : IFitnessFactory
    {
        private int _numberOfObjectives { get; }
        private static readonly NonDominatedParetoComparer FitnessComparer = new NonDominatedParetoComparer();

        public ParetoMultiobjective(int numberOfObjectives)
        {
            _numberOfObjectives = numberOfObjectives;
        }

        public IFitness GetFitness()
        {
             return new MultiObjectiveFitness(_numberOfObjectives);
        }

        public IFitnessComparer GetFitnessComparer()
        {
            return FitnessComparer;
        }
    }
}
