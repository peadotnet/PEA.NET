using Pea.Core;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Entity.Implementation.MultiChromosome
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(IList<KeyValuePair<string, IChromosomeFactory>> chromosomeFactories, IRandom random)
        {
            foreach (var factory in chromosomeFactories)
            {
                var mutations = factory.Value.GetMutations();
                var mutationProvider = ProviderFactory.Create<IMutation>(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(factory.Key, mutationProvider);
            }
        }

        public IList<IEntity> Mutate(IList<IEntity> entities)
        {
            var result = new List<IEntity>();
            foreach (var entity in entities)
            {
                result.Add(MutateEntity(entity));
            }

            return result;
        }

        public IEntity MutateEntity(IEntity entity)
        {
            var mutatedEntity = (IEntity)entity.Clone();

            foreach (var chromosome in entity.Genotype.Chromosomes)
            {
                if (MutationProviders.ContainsKey(chromosome.Key))
                {
                    var provider = MutationProviders[chromosome.Key];
                    var mutation = provider.GetOne();

                    var mutatedChromosome = mutation.Mutate(chromosome.Value);

                    mutatedEntity.Genotype.Chromosomes[chromosome.Key] = mutatedChromosome;
                }
            }

            return mutatedEntity;
        }
    }
}
