using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Core.Settings;

namespace Pea.Core.Entity
{
    public class EntityCrossover : IEntityCrossover
    {
        public Dictionary<string, IProvider<ICrossover>> CrossoverProviders { get; }

        public EntityCrossover(IList<PeaSettingsNamedType> chromosomeFactories, IRandom random)
        {
            foreach (var factory in chromosomeFactories)
            {
                var factoryInstance = Activator.CreateInstance(factory.ValueType) as IChromosomeFactory;
                var crossovers = factoryInstance.GetCrossovers();
                var mutationProvider = ProviderFactory.Create<ICrossover>(crossovers.Count(), random);
                foreach (var crossover in crossovers)
                {
                    mutationProvider.Add(crossover, 1.0);
                }

                CrossoverProviders.Add(factory.Names[0], mutationProvider);
            }
        }

        public IList<IEntity> Cross(IList<IEntity> parents)
        {
            if (parents.Count < 2) throw new ArgumentException(nameof(parents));

            var child1 = (IEntity)parents[0].Clone();
            var child2 = (IEntity)parents[1].Clone();

            foreach (var chromosomeName in parents[0].Genotype.Chromosomes.Keys)
            {
                var parent1Chromosome = parents[0].Genotype.Chromosomes[chromosomeName];
                var parent2Chromosome = parents[1].Genotype.Chromosomes[chromosomeName];
                var parentChromosomes = new List<IChromosome>()
                {
                    parent1Chromosome,
                    parent2Chromosome
                };

                if (CrossoverProviders.ContainsKey(chromosomeName))
                {
                    var provider = CrossoverProviders[chromosomeName];
                    var crossover = provider.GetOne();

                    var mutatedChromosomes = crossover.Cross(parentChromosomes);

                    child1.Genotype.Chromosomes[chromosomeName] = mutatedChromosomes[0];
                    child2.Genotype.Chromosomes[chromosomeName] = mutatedChromosomes[1];
                }
            }

            var childs = new List<IEntity>()
            {
                child1,
                child2
            };

            return childs;
        }
    }
}
