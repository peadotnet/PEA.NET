using Pea.Population;
using System;
using System.Collections.Generic;

namespace Pea.Core.Entity
{
    public class EntityBase : IEntity, IPopulationEntity
    {
        public MultiKey OriginIslandKey { get; set; }
        public IDictionary<string, IChromosome> Chromosomes { get; set; }
        public IFitness Fitness { get; private set; }
        public Dictionary<string, string> LastCrossOvers { get; set; } = new Dictionary<string, string>(2);
        public Dictionary<string, string> LastMutations { get; set; } = new Dictionary<string, string>(1);
        public int IndexInList { get; set; }
        public int IndexInPopulation { get; set; }

        public EntityBase(int chromosomesCount)
		{
            Chromosomes = new Dictionary<string, IChromosome>(chromosomesCount);
        }

        public virtual IEntity Clone(bool cloneChromosomes)
        {
            var clone = (EntityBase)Activator.CreateInstance(this.GetType());
            clone.IndexInList = this.IndexInList;
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
