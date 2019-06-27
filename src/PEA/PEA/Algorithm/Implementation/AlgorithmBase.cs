﻿using System.Collections.Generic;
using Pea.Core;

namespace Pea.Algorithm.Implementation
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        public delegate IList<IEntity> EvaluationDelegate(IList<IEntity> entityList);

        public IEngine Engine { get; }
        public IPopulation Population { get; set; }

        public abstract void InitPopulation();
        public abstract void RunOnce();

        private EvaluationDelegate _evaluate;


        protected AlgorithmBase(IEngine engine, EvaluationDelegate evaluate)
        {
            Engine = engine;
            _evaluate = evaluate;
        }

        protected IEntity CreateEntity()
        {
            var entityCreator = Engine.EntityCreators.GetOne();
            var entity = entityCreator.CreateEntity();
            return entity;
        }

        protected void Evaluate(IList<IEntity> entities)
        {
            _evaluate(entities);
            //foreach (IEntity entity in entities)
            //{
            //    entity.Fitness = Engine.FitnessCalculator.CalculateFitness(entity.Phenotype);
            //}
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