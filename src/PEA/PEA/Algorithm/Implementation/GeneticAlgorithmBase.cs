using Pea.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pea.Algorithm.Implementation
{
    public abstract class GeneticAlgorithmBase : IGeneticAlgorithm
    {
        public IEngine Engine { get; }
        public IPopulation Population { get; set; }
        public IStopCriteria StopCriteria { get; set; }


        public virtual void InitPopulation(EntityList? entityList = null)
        {
            var fitnessLength = Engine.Parameters.GetInt(ParameterNames.FitnessLength);
            var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
            var minNumberOfEntities = Convert.ToInt32(Engine.Parameters.GetValue(ParameterNames.SelectionRate) * maxNumberOfEntities);

            if (entityList == null) entityList = new EntityList(maxNumberOfEntities);

            Population = new Population.Population(fitnessLength, minNumberOfEntities, maxNumberOfEntities);

            int timeOut = Engine.Parameters.GetInt(Core.ParameterNames.PopulationInitTimeout);
            var cancellationSource = new CancellationTokenSource(timeOut);
            var ct = cancellationSource.Token;

            try
            {
                var task = new Task(() =>
                {
                    while (entityList.Count < maxNumberOfEntities)
                    {
                        var entity = CreateEntity();

                        if (entity != null) entityList.Add(entity);

                        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
                    }
                });

                task.RunSynchronously();
            }
            catch (OperationCanceledException e)
            {
                System.Diagnostics.Debug.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                cancellationSource.Dispose();
            }

            Evaluate(entityList);
            Population.AddRange(entityList);
            MergeToBests(Population);
        }


        public abstract StopDecision RunOnce();

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