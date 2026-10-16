using Pea.Chromosome;
using Pea.Fitness;
using Pea.Population;
using System;
using System.Collections.Generic;

namespace Pea.Entity
{
    public class EntityBase
    {
        public MultiKey OriginIslandKey { get; set; }
        public IDictionary<string, IChromosome> Chromosomes { get; set; }
        public IFitness Fitness { get; private set; }
        internal Dictionary<string, string> LastCrossOvers { get; set; } = new Dictionary<string, string>(2);
        internal Dictionary<string, string> LastMutations { get; set; } = new Dictionary<string, string>(1);
        internal int IndexInList { get; set; }
        internal int IndexInPopulation { get; set; }

        public EntityBase(int chromosomesCount)
		{
            Chromosomes = new Dictionary<string, IChromosome>(chromosomesCount);
        }

        public virtual EntityBase Clone(bool cloneChromosomes)
        {
            var clone = (EntityBase)Activator.CreateInstance(GetType());
            clone.IndexInList = IndexInList;
            clone.IndexInPopulation = IndexInPopulation;
            clone.OriginIslandKey = OriginIslandKey;

            if (cloneChromosomes)
            {
                foreach (var key in Chromosomes.Keys)
                {
                    clone.Chromosomes.Add(key, Chromosomes[key].DeepClone());
                }
            }

            return clone;
        }

        public void SetFitness(IFitness fitness)
		{
            fitness.Entity = this;
            Fitness = fitness;
		}
    }
}
