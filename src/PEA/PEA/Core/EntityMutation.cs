using System;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(IList<KeyValuePair<string, IChromosomeFactory>> chromosomeFactories, IRandom random)
        {
            foreach (var factory in chromosomeFactories)
            {
                var mutations = factory.Value.GetMutations();
                var mutationProvider = CreateProvider(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(factory.Key, mutationProvider);
            }
        }

        public IProvider<IMutation> CreateProvider(int count, IRandom random)
        {
            if (count < 2) return new SimpleProvider<IMutation>();

            return new StochasticProvider<IMutation>(random);
        }

        public IList<IEntity> Mutate(IList<IEntity> entities)
        {
            throw new NotImplementedException();
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
