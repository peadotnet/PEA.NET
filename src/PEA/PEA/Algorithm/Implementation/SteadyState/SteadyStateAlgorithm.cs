using Pea.Core;
using System.Threading;

namespace Pea.Algorithm.Implementation
{
    public class SteadyStateAlgorithm : GeneticAlgorithmBase
    {
        public SteadyStateAlgorithm(IEngine engine) : base(engine)
        {
        }

        public override StopDecision RunOnce()
        {
            var parents = SelectParents(Population, 2);
            var offspring = Crossover(parents, 2);
            var mutated = Mutate(offspring);
            var evaluated = Evaluate(mutated);
            var inserted = Reinsert(Population, evaluated, parents, Population);
            MergeToBests(inserted);
            return StopCriteria.MakeDecision(this.Engine, this.Population);

        }
    }
}
