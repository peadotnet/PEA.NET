using Pea.Configuration.Implementation;
using Pea.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pea.Algorithm.Implementation
{
    public abstract class GeneticAlgorithmBase : IGeneticAlgorithm
    {
        public IPopulation Population { get; private set; }
        public IStopCriteria StopCriteria { get; set; }

        protected ParameterSet Parameters { get; } = new ParameterSet();
        public IProvider<IEntityCreator> EntityCreators { get; }
        public IProvider<ISelection> Selections { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public IFitnessComparer FitnessComparer { get; set; }
        public IProvider<IReplacement> Replacements { get; set; }

        private Action<IEntityList> _mergeToBests;


        public virtual void InitPopulation(EntityList? entityList = null)
        {
            var fitnessLength = Parameters.GetInt(ParameterNames.FitnessLength);
            var maxNumberOfEntities = Parameters.GetInt(ParameterNames.PopulationSize);
            var minNumberOfEntities = Convert.ToInt32(Parameters.GetValue(ParameterNames.SelectionRate) * maxNumberOfEntities);

            if (entityList == null) entityList = new EntityList(maxNumberOfEntities);

            Population = new Population.Population(fitnessLength, minNumberOfEntities, maxNumberOfEntities);

            int timeOut = Parameters.GetInt(Core.ParameterNames.PopulationInitTimeout);
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


        protected GeneticAlgorithmBase(ParameterSet parameters, IProvider<IEntityCreator> entityCreators, Action<IEntityList> mergeToBests)
        {
            Parameters = parameters;
            EntityCreators = entityCreators;
            _mergeToBests = mergeToBests;
        }

        public void SetEvaluationCallback(EvaluationDelegate evaluationCallback)
        {
            _evaluate = evaluationCallback;
        }


        protected IEntity CreateEntity()
        {
            var creator = EntityCreators.GetOne();
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
            if (_mergeToBests != null) _mergeToBests(entities);
        }

        protected IEntityList SelectParents(IEntityList entities, int count)
        {
            var selector = Selections.GetOne();
            var parents = selector.Select(entities, count);
            return parents;
        }

        protected IEntityList Crossover(IEntityList parents, int count)
        {
            var children = EntityCrossover.Cross(parents, count);
            return children;
        }

        protected IEntityList Mutate(IEntityList children)
        {
            children = EntityMutation.Mutate(children);
            return children;
        }

        protected IEntityList Reinsert(IPopulation targetPopulation, IEntityList offsprings, IEntityList parents, IPopulation sourcePopulation)
        {
            var replacement = Replacements.GetOne();
            return replacement.Replace(targetPopulation, offsprings, parents, sourcePopulation);
        }
    }
}