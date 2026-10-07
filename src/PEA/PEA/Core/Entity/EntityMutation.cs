using System.Collections.Generic;
using System.Linq;

namespace Pea.Core.Entity
{
    public class EntityMutation : IEntityMutation
    {
        private readonly string[] _chromosomeNames;

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

            _chromosomeNames = MutationProviders.Keys.ToArray();
        }

        void IEntityMutation.Mutate(IEntityList entities)
        {
            for (int i=0; i< entities.Count; i++)
            { 
                MutateEntity(entities[i]);
            }
        }

        internal void MutateEntity(EntityBase entity)
        {
            for (int i = 0; i < _chromosomeNames.Length; i++)
            {
                var key = _chromosomeNames[i];
                var chromosome = entity.Chromosomes[key];
                IChromosome mutatedChromosome = null;
                IMutation mutation = null;

                var provider = MutationProviders[key];

                while (mutatedChromosome == null)
                {
                    mutation = provider.GetOne();
                    mutatedChromosome = mutation.Mutate(chromosome);
                }

                entity.Chromosomes[key] = mutatedChromosome;
                entity.LastMutations[key] = mutation.GetType().Name;
            }
        }
    }
}
