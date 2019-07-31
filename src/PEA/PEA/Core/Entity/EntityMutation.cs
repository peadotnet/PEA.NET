using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core.Settings;

namespace Pea.Core.Entity
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(IList<PeaSettingsNamedType> chromosomeFactories, IRandom random, ParameterSet parameterSet, IConflictDetector conflictDetector)
        {
            foreach (var factory in chromosomeFactories)
            {
                var factoryInstance = Activator.CreateInstance(factory.ValueType, random, parameterSet, conflictDetector) as IChromosomeFactory;
                var mutations = factoryInstance.GetMutations();
                var mutationProvider = ProviderFactory.Create<IMutation>(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(factory.Keys[0], mutationProvider);
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
                    var mutation = provider.GetOne();

                    var mutatedChromosome = mutation.Mutate(chromosome.Value);
                    if (mutatedChromosome == null) return null;


                    var conflictedPositions = 
                        SortedSubsetChromosomeValidator.SearchForConflict(((SortedSubsetChromosome)chromosome.Value).Sections);
                    if (conflictedPositions.Count > 0)
                    {
                        bool error = true;  //For breakpoint
                    }

                    mutatedEntity.Chromosomes[chromosome.Key] = mutatedChromosome;
                    mutatedEntity.LastMutations.Add(chromosome.Key, mutation.GetType().Name);
                }
            }

            return mutatedEntity;
        }
    }
}
