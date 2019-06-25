using System.Collections.Generic;
using Pea.Core;

namespace Pea.Algorithm.Implementation
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        public delegate IList<IEntity> DecodePhenotypesDelegate(IList<IEntity> entityList);
        public delegate IList<IEntity> AssessFitnessDelegate(IList<IEntity> entityList);

        public IEngine Engine { get; }
        public IPopulation Population { get; set; }

        public abstract void InitPopulation();
        public abstract void RunOnce();

        private DecodePhenotypesDelegate _decodePhenotypes;
        private AssessFitnessDelegate _assessFitness;


        protected AlgorithmBase(IPopulation population, IEngine engine, 
            DecodePhenotypesDelegate decodePhenotypes, AssessFitnessDelegate assessFitness)
        {
            Population = population;
            Engine = engine;
            _decodePhenotypes = decodePhenotypes;
            _assessFitness = assessFitness;
        }

        protected IEntity CreateEntity()
        {
            var entityCreator = Engine.EntityCreators.GetOne();
            var entity = entityCreator.CreateEntity();
            return entity;
        }

        protected void DecodePhenotypes(IList<IEntity> entities)
        {
            _decodePhenotypes(entities);
            //foreach (IEntity entity in entities)
            //{
            //    entity.Phenotype = Engine.PhenotypeDecoder.Decode(entity.Genotype);
            //}
        }

        protected void AssessFitness(IList<IEntity> entities)
        {
            _assessFitness(entities);
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
