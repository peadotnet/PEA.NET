using Pea.Core;
using System;

namespace Pea.Algorithm.Implementation
{
    public class SteadyStateAlgorithm : GeneticAlgorithmBase
    {
        public SteadyStateAlgorithm(ParameterSet parameters, IProvider<IEntityCreator> entityCreators, Action<IEntityList> mergeToBests) : base(parameters, entityCreators, mergeToBests)
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
            return StopCriteria.MakeDecision(Population, FitnessComparer);
        }
    }
}
