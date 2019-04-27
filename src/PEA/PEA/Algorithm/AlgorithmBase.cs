using Pea.Core;
using System.Collections.Generic;

namespace Pea.Algorithm
{
    public abstract class AlgorithmBase : IAlgorithm
    {
        public StochasticProvider<IEntityCreator> EntityCreators { get; set; }
        public StochasticProvider<ISelection> Selectors { get; set; }
        public IPhenotypeDecoder fenotypeDecoder { get; set; }
        public IFitnessCalculator fitnessCalculator { get; set; }
        public IPopulation Population { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public StochasticProvider<IReplacement> Replacements { get; set; }

        public abstract void InitPopulation();

        public abstract void RunOnce();

        protected IEntity CreateEntity()
        {
            var entityCreator = EntityCreators.ChooseOne();
            var entity = entityCreator.CreateEntity();
            return entity;
        }

        protected void DecodePhenotypes(IList<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                entity.Phenotype = fenotypeDecoder.Decode(entity.Genotype);
            }
        }

        protected void AssessFitness(IList<IEntity> entities)
        {
            foreach (IEntity entity in entities)
            {
                entity.Fitness = fitnessCalculator.CalculateFitness(entity.Phenotype);
                //TODO: implement Population.Best
            }
        }

        protected IList<IEntity> SelectParents(IList<IEntity> entities)
        {
            var selector = Selectors.ChooseOne();
            var parents = selector.Select(entities);
            return parents;
        }

        protected IList<IEntity> Crossover(IList<IEntity> parents)
        {
            var children = EntityCrossover.Cross(parents);
            return children;
        }

        protected IList<IEntity> Mutate(IList<IEntity> children)
        {
            children = EntityMutation.Mutate(children);
            return children;
        }

        protected IList<IEntity> Replace(IList<IEntity> target, IList<IEntity> children)
        {
            var replacement = Replacements.ChooseOne();
            var result = replacement.Replace(target, children);
            return result;
        }
    }
}
