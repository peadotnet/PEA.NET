using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core.Entity
{
    public class EntityCreator : IEntityCreator
    {
        public Dictionary<string, IProvider<IChromosomeCreator>> CreatorProviders { get; } = new Dictionary<string, IProvider<IChromosomeCreator>>();


        public EntityCreator(List<SubProblem> subProblemList, IDictionary<string, IList<IConflictDetector>> conflictDetectors, IRandom random)
        {
            for (int i = 0; i < subProblemList.Count; i++)
            {
                var subProblem = subProblemList[i];

                var factoryInstance = Activator.CreateInstance(subProblem.Encoding.ChromosomeType, random, subProblem.ParameterSet, conflictDetectors[subProblem.Encoding.Key]) as IChromosomeFactory;

                var creators = factoryInstance.GetCreators();
                var creatorProvider = ProviderFactory.Create<IChromosomeCreator>(creators.Count, random);
                for (int c = 0; c < creators.Count; c++)
                {
                    var creator = creators[i];
                    creatorProvider.Add(creator, 1.0);
                }

                CreatorProviders.Add(subProblem.Encoding.Key, creatorProvider);
            }
        }

        public virtual void Init(IEvaluationInitData initData)
        {

        }

        public virtual IEntity CreateEntity()
        {
            //TODO: set islandKey

            IEntity entity = new Entity();
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
