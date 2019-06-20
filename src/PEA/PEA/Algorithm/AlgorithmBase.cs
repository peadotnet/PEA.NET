using System.Collections.Generic;
using Pea.Core;
using Pea.Island;

namespace Pea.Algorithm
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        public IEngine Engine { get; }
        public IPopulation Population { get; set; }

        public abstract void InitPopulation();
        public abstract void RunOnce();

        protected AlgorithmBase(IPopulation population, IslandEngine engine)
        {
            Population = population;
            Engine = engine;
        }

        protected IEntity CreateEntity()
        {
            var entityCreator = Engine.EntityCreators.GetOne();
            var entity = entityCreator.CreateEntity();
            return entity;
        }

        protected void DecodePhenotypes(IList<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                entity.Phenotype = Engine.PhenotypeDecoder.Decode(entity.Genotype);
            }
        }

        protected void AssessFitness(IList<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                entity.Fitness = Engine.FitnessCalculator.CalculateFitness(entity.Phenotype);
            }
        }

        protected void MergeToBests(IList<IEntity> entities)
        {
            foreach (var entity in entities)
            {
                Population.Bests = Engine.FitnessComparer.MergeToBests(Population.Bests, entity);
            }
        }

        protected IList<IEntity> SelectParents(IList<IEntity> entities)
        {
            var selector = Engine.Selections.GetOne();
            var parents = selector.Select(entities);
            return parents;
        }

        protected IList<IEntity> Crossover(IList<IEntity> parents)
        {
            var children = Engine.EntityCrossover.Cross(parents);
            return children;
        }

        protected IList<IEntity> Mutate(IList<IEntity> children)
        {
            children = Engine.EntityMutation.Mutate(children);
            return children;
        }

        protected void Reinsert(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            var replacement = Engine.Reinsertions.GetOne();
            replacement.Reinsert(targetPopulation, offspring, parents, sourcePopulation);
        }
    }
}
