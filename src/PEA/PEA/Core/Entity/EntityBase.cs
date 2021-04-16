using System;
using System.Collections.Generic;

namespace Pea.Core.Entity
{
    public class EntityBase : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; set; }
        public IDictionary<string, IChromosome> Chromosomes { get; set; } = new Dictionary<string, IChromosome>();
        public IFitness Fitness { get; private set; }
        public Dictionary<string, string> LastCrossOvers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LastMutations { get; set; } = new Dictionary<string, string>();

        public virtual object Clone()
        {
            var clone = (EntityBase)Activator.CreateInstance(this.GetType());
            clone.IndexOfList = this.IndexOfList;
            clone.OriginIslandKey = this.OriginIslandKey;

            foreach (var key in Chromosomes.Keys)
            {
                clone.Chromosomes.Add(key, this.Chromosomes[key].DeepClone());
            }

            return clone;
        }

        public void SetFitness(IFitness fitness)
		{
            if (fitness.Entity != null) throw new AlgorithmException("The Fitness object is already assigned to another Entity. This would lead to shared state error.");
            fitness.Entity = this;
            Fitness = fitness;
		}
    }
}
