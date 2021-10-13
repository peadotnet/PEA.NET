using System.Collections.Generic;
using Pea.Core;

namespace Pea.Algorithm.Implementation
{
    public abstract class GeneticAlgorithmBase : IGeneticAlgorithm
    {
        public IEngine Engine { get; }
        public IPopulation Population { get; set; }

        public abstract void InitPopulation();
        public abstract void RunOnce();

        private EvaluationDelegate _evaluate;

        protected GeneticAlgorithmBase(IEngine engine)
        {
            Engine = engine;
        }

        public void SetEvaluationCallback(EvaluationDelegate evaluationCallback)
        {
            _evaluate = evaluationCallback;
        }

        protected IEntity CreateEntity()
        {
            var creator = Engine.EntityCreators.GetOne();
            var entity = creator.CreateEntity();
            return entity;
        }

        protected IEntityList Evaluate(IEntityList population)
        {
            if (population.Count == 0) return population;

            return _evaluate(population);
        }

        protected void MergeToBests(IEntityList entities)
        {
            Engine.MergeToBests(entities);
        }

        protected IEntityList SelectParents(IEntityList entities, int count)
        {
            var selector = Engine.Selections.GetOne();
            var parents = selector.Select(entities, count);
            return parents;
        }

        protected IEntityList Crossover(IEntityList parents, int count)
        {
            var children = Engine.EntityCrossover.Cross(parents, count);
            return children;
        }

        protected IEntityList Mutate(IEntityList children)
        {
            children = Engine.EntityMutation.Mutate(children);
            return children;
        }

        protected IEntityList Reinsert(IPopulation targetPopulation, IEntityList offsprings, IEntityList parents, IPopulation sourcePopulation)
        {
            var replacement = Engine.Replacements.GetOne();
            return replacement.Replace(targetPopulation, offsprings, parents, sourcePopulation);
        }
    }
}