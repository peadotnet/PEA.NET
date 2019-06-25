﻿using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Core.Settings;

namespace Pea.Core.Entity
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(IList<PeaSettingsNamedType> chromosomeFactories, IRandom random)
        {
            foreach (var factory in chromosomeFactories)
            {
                var factoryInstance = Activator.CreateInstance(factory.ValueType) as IChromosomeFactory;
                var mutations = factoryInstance.GetMutations();
                var mutationProvider = ProviderFactory.Create<IMutation>(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(factory.Names[0], mutationProvider);
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
