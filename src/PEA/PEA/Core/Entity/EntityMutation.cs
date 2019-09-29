using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Configuration.Implementation;
using Pea.Core.Settings;

namespace Pea.Core.Entity
{
    public class EntityMutation : IEntityMutation
    {
        public Dictionary<string, IProvider<IMutation>> MutationProviders { get; } = new Dictionary<string, IProvider<IMutation>>();

        public EntityMutation(List<SubProblem> subProblemList, IRandom random)
        {
            foreach (var subProblem in subProblemList)
            {
                var factoryInstance = Activator.CreateInstance(subProblem.Encoding.ChromosomeType, random, subProblem.ParameterSet, subProblem.ConflictDetectors) as IChromosomeFactory;
                var mutations = factoryInstance.GetMutations();
                var mutationProvider = ProviderFactory.Create<IMutation>(mutations.Count(), random);
                foreach (var mutation in mutations)
                {
                    mutationProvider.Add(mutation, 1.0);
                }

                MutationProviders.Add(subProblem.Encoding.Key, mutationProvider);
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

                    var mutatedChromosome = mutation.Mutate(chromosome.Value.DeepClone());
                    if (mutatedChromosome == null) return null;

                    //TODO: Delete this
                    var conflictedPositions =
                        SortedSubsetChromosomeValidator.SearchForConflict(((SortedSubsetChromosome)mutatedChromosome).Sections);
                    if (conflictedPositions.Count > 0)
                    {
                        bool error = true;  //For breakpoint
                        throw new ApplicationException($"Conflict between neighboring values! (Mutation: {mutation.GetType().Name})");
                    }

                    mutatedEntity.Chromosomes[chromosome.Key] = mutatedChromosome;
                    mutatedEntity.LastMutations.Add(chromosome.Key, mutation.GetType().Name);
                }
            }

            return mutatedEntity;
        }
    }
}
