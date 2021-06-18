using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;

namespace Pea.Fitness
{
    public class ParetoMultiobjective : IFitnessFactory
    {
        public IEntity Entity { get; internal set; }

        private int _numberOfObjectives { get; }
        private static readonly IFitnessComparer FitnessComparer = new ParetoComparerWithConstraintViolationReduction();

        public ParetoMultiobjective() : this(1)
        {
        }

        public ParetoMultiobjective(int numberOfObjectives)
        {
            _numberOfObjectives = numberOfObjectives;
        }

        public IFitness GetFitness()
        {
             return new MultiObjectiveFitness(new double[_numberOfObjectives]);
        }

        public IFitnessComparer GetFitnessComparer()
        {
            return FitnessComparer;
        }
    }
}
