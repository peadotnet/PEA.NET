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

            Population.Entities = Evaluate(Population.Entities);
            MergeToBests(Population.Entities);
        }

        public override void RunOnce()
        {
            var parents = SelectParents(Population.Entities);
            var children = Crossover(parents);
            children = Mutate(children);
            children = Evaluate(children);
            //TODO: Reduction (children) ?
            MergeToBests(children);
            Reinsert(Population.Entities, children, parents, Population.Entities);
        }
    }
}
