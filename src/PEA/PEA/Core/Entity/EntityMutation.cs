using System.Collections.Generic;
using System.Linq;

namespace Pea.Core.Entity
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(IDictionary<string, IChromosomeFactory> chromosomeFactories, IRandom random)
        {
            foreach (var key in chromosomeFactories.Keys)
            {
                var factory = chromosomeFactories[key];

                var mutations = factory.GetMutations();
                var mutationProvider = ProviderFactory.Create<IMutation>(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(key, mutationProvider);
            }
        }

        public IList<IEntity> Mutate(IList<IEntity> entities)
        {
            var result = new List<IEntity>();
            foreach (var entity in entities)
            {
                var mutated = MutateEntity(entity);
                if (mutated != null) result.Add(mutated);
            }
            return result;
        }

        public IEntity MutateEntity(IEntity entity)
        {
            var mutatedEntity = (IEntity)entity.Clone();
            mutatedEntity.LastCrossOvers = entity.LastCrossOvers;

            foreach (var chromosome in entity.Chromosomes)
            {
                if (MutationProviders.ContainsKey(chromosome.Key))
                {
                    var provider = MutationProviders[chromosome.Key];
                    var retryCount = provider.Count();
                    var mutation = provider.GetOne();

                    var mutatedChromosome = mutation.Mutate(chromosome.Value);
                    if (mutatedChromosome == null) return null;

                    mutatedEntity.Chromosomes[chromosome.Key] = mutatedChromosome;
                    mutatedEntity.LastMutations.Add(chromosome.Key, mutation.GetType().Name);
                }
            }

            return mutatedEntity;
        }
    }
}
