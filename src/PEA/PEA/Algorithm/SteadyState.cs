using System;

namespace Pea.Algorithm
{
    public class SteadyState : AlgorithmBase
    {
        public override void InitPopulation()
        {
            //TODO: max entity count parameter
            for (int i = 0; i < Population.MaxNumberOfEntities; i++)
            {
                var entity = CreateEntity();
                Population.Add(entity);
            }

            DecodePhenotypes(Population.Entities);
            AssessFitness(Population.Entities);
        }

        public override void RunOnce()
        {
            var parents = SelectParents(Population.Entities);
            var children = Crossover(parents);
            children = Mutate(children);
            DecodePhenotypes(children);
            AssessFitness(children);
            Population.Entities = Replace(Population.Entities, children);
        }
    }
}
