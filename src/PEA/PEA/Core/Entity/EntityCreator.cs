using System;
using System.Collections.Generic;

namespace Pea.Core.Entity
{
    public class EntityCreator : IEntityCreator
    {

        public Type EntityType { get; }

        public Dictionary<string, IProvider<IChromosomeCreator>> CreatorProviders { get; } = new Dictionary<string, IProvider<IChromosomeCreator>>();

        
        public EntityCreator(Type entityType, IDictionary<string, IChromosomeFactory> chromosomeFactories, IRandom random)
        {
            EntityType = entityType;

            foreach (var key in chromosomeFactories.Keys)
            {
                var factory = chromosomeFactories[key];
                var creators = factory.GetCreators();
                var creatorProvider = ProviderFactory.Create<IChromosomeCreator>(creators.Count, random);

                foreach (var creator in creators)
                {
                    creatorProvider.Add(creator, 1.0);
                }

                CreatorProviders.Add(key, creatorProvider);
            }
        }

        public virtual void Init(IEvaluationInitData initData)
        {

        }

        public virtual IEntity CreateEntity()
        {
            //TODO: set islandKey

            IEntity entity = (IEntity)Activator.CreateInstance(EntityType);
            foreach (var key in CreatorProviders.Keys)
            {
                var provider = CreatorProviders[key];
                var creator = provider.GetOne();

                var chromosome = creator.Create();
                entity.Chromosomes.Add(key, chromosome);
            }

            return entity;
        }
    }
}
