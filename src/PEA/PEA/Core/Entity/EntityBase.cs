using Pea.Population;
using System;
using System.Collections.Generic;

namespace Pea.Core.Entity
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
            var clone = (EntityBase)Activator.CreateInstance(this.GetType());
            clone.IndexInList = this.IndexInList;
            clone.IndexInPopulation = this.IndexInPopulation;
            clone.OriginIslandKey = this.OriginIslandKey;

            if (cloneChromosomes)
            {
                foreach (var key in Chromosomes.Keys)
                {
                    clone.Chromosomes.Add(key, this.Chromosomes[key].DeepClone());
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
