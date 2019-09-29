using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;
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
        public abstract IList<Type> GetSelections();
        public abstract IList<Type> GetReinsertions();
        public abstract IEnumerable<PeaSettingsNamedValue> GetParameters();

        private EvaluationDelegate _evaluate;


        protected AlgorithmBase(IEngine engine)
        {
            Engine = engine;
        }

        public void SetEvaluationCallback(EvaluationDelegate evaluationCallback)
        {
            _evaluate = evaluationCallback;
        }

        protected IEntity CreateEntity()
        {
            var entity = Engine.EntityCreator.CreateEntity();
            return entity;
        }

        protected IList<IEntity> Evaluate(IList<IEntity> entities)
        {
            return _evaluate(entities);
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
            var mutationProbability = Engine.Parameters.GetValue(ParameterNames.MutationProbability);
            var rnd = Engine.Random.GetDouble(0, 1);
            if (rnd < mutationProbability)
            {
                children = Engine.EntityMutation.Mutate(children);
            }
            return children;
        }

        protected void Reinsert(IList<IEntity> targetPopulation, IList<IEntity> offspring, IList<IEntity> parents, IList<IEntity> sourcePopulation)
        {
            var replacement = Engine.Reinsertions.GetOne();
            replacement.Reinsert(targetPopulation, offspring, parents, sourcePopulation);
        }
    }
}
