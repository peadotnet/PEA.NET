using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Pea.Core.Entity
{
    public class EntityCrossover : IEntityCrossover
    {
        public Dictionary<string, IProvider<ICrossover>> CrossoverProviders { get; } = new Dictionary<string, IProvider<ICrossover>>();
        IRandom _random;

        public EntityCrossover(IDictionary<string, IChromosomeFactory> chromosomeFactories, IRandom random)
        {
            _random = random;
            foreach (var key in chromosomeFactories.Keys)
            {
                var factory = chromosomeFactories[key];

                var crossovers = factory.GetCrossovers();
                var crossoverProvider = ProviderFactory.Create<ICrossover>(crossovers.Count(), random);
                foreach (var crossover in crossovers)
                {
                    crossoverProvider.Add(crossover, 1.0);
                }

                CrossoverProviders.Add(key, crossoverProvider);
            }
        }

        public IEntityList Cross(IEntityList parents, int count)
        {
            if (parents.Count < 2) throw new ArgumentException(nameof(parents));

            var offsprings = new EntityList(count);

            while (offsprings.Count < count)
            {
                int p0 = _random.GetInt(0, parents.Count);
                int p1 = _random.GetIntWithTabu(0, parents.Count, p0);

                var parent0 = parents[p0];
                var parent1 = parents[p1];
                IEntity offspring0 = parent0.Clone(false);
                IEntity offspring1 = parent1.Clone(false);

                double crossoverProbability = parents[0].Chromosomes.Keys.Count * 1.5;

                bool offspring1Failed = false;

                foreach (var chromosomeName in parents[0].Chromosomes.Keys)
                {
                    IList<IChromosome> crossoveredChromosomes = new List<IChromosome>(parent0.Chromosomes.Count);
                    while (crossoveredChromosomes.Count == 0)
                    {
                        var crossoverDecision = _random.GetDouble(0, crossoverProbability);
                        if (crossoverDecision > 1) continue;

                        var provider = CrossoverProviders[chromosomeName];
                        var crossover = provider.GetOne();

                        try
                        {
                            crossoveredChromosomes = crossover.Cross(parent0.Chromosomes[chromosomeName], parent1.Chromosomes[chromosomeName]);
                            if (crossoveredChromosomes.Count > 0)
                            {
                                var crossoverName = crossover.GetType().Name;
                                offspring0.Chromosomes.Add(chromosomeName, crossoveredChromosomes[0]);
                                offspring0.LastCrossOvers.Add(chromosomeName, crossoverName);

                                if (crossoveredChromosomes.Count > 1)
                                {
                                    if (!offspring1Failed) offspring1.Chromosomes.Add(chromosomeName, crossoveredChromosomes[1]);
                                    offspring1.LastCrossOvers.Add(chromosomeName, crossoverName);
                                }
                                else
                                {
                                    offspring1Failed = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e);
                        }
                    }
                }
                offsprings.Add(offspring0);
                if (!offspring1Failed) offsprings.Add(offspring1);
            }

            return offsprings;
        }
    }
}
