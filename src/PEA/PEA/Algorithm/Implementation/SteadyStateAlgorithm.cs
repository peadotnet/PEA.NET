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
            Population = new Population.Population();

            var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.MaxNumberOfEntities);
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
            var parents = SelectParents(Population.Entities);
            var offspring = Crossover(parents);
            var mutated = Mutate(offspring);
            var evaluated = Evaluate(mutated);

            if (evaluated[0] == null)
			{
                bool brk = true;
			}
            //TODO: Reduction (children) ?
            var inserted = Reinsert(Population.Entities, evaluated, parents, Population.Entities);
            MergeToBests(inserted);
        }
    }
}
