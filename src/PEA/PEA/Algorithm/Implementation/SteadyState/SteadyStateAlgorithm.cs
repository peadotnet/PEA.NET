using Pea.Core;

namespace Pea.Algorithm.Implementation
{
    public class SteadyStateAlgorithm : GeneticAlgorithmBase
    {
        public SteadyStateAlgorithm(IEngine engine) : base(engine)
        {
        }

        public override void InitPopulation()
        {
            var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
            var minNumberOfEntities = System.Convert.ToInt32(Engine.Parameters.GetValue(ParameterNames.SelectionRate) * maxNumberOfEntities);

            Population = new Population.Population(minNumberOfEntities, maxNumberOfEntities + 2);

            for (int i = 0; i < maxNumberOfEntities; i++)
            {
                var entity = CreateEntity();
                Population.Add(entity);
            }

            Evaluate(Population.Entities);
            MergeToBests(Population.Entities);
        }

        public override void RunOnce()
        {
            var parents = SelectParents(Population.Entities, 2);
            var offspring = Crossover(parents, 2);
            var mutated = Mutate(offspring);
            var evaluated = Evaluate(mutated);
            var inserted = Reinsert(Population.Entities, evaluated, parents, Population.Entities);
            MergeToBests(inserted);
        }
    }
}
