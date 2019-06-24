using Pea.Core;
using Pea.Island;

namespace Pea.Algorithm
{
    public class SteadyState : AlgorithmBase
    {
        public SteadyState(IPopulation population, IEngine engine, 
            DecodePhenotypesDelegate decodePhenotypes, AssessFitnessDelegate assessFitness) : base(population, engine, decodePhenotypes, assessFitness)
        {
        }

        public override void InitPopulation()
        {
            for (int i = 0; i < Population.MaxNumberOfEntities; i++)
            {
                var entity = CreateEntity();
                Population.Add(entity);
            }

            DecodePhenotypes(Population.Entities);
            AssessFitness(Population.Entities);
            MergeToBests(Population.Entities);
        }

        public override void RunOnce()
        {
            var parents = SelectParents(Population.Entities);
            var children = Crossover(parents);
            children = Mutate(children);
            DecodePhenotypes(children);
            AssessFitness(children);
            MergeToBests(children);
            Reinsert(Population.Entities, children, parents, Population.Entities);
        }
    }
}
