using System.Collections.Generic;

namespace Pea.Core.Entity
{
    public class Entity : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; set; }
        public IDictionary<string, IChromosome> Chromosomes { get; } = new Dictionary<string, IChromosome>();
        public IFitness Fitness { get; set; }
        public Dictionary<string, string> LastCrossOvers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LastMutations { get; set; } = new Dictionary<string, string>();

        public object Clone()
        {
            var clone = new Entity()
            {
                IndexOfList = this.IndexOfList,
                OriginIslandKey = this.OriginIslandKey,
                Fitness = this.Fitness
            };

            foreach (var key in Chromosomes.Keys)
            {
                clone.Chromosomes.Add(key, this.Chromosomes[key].DeepClone());
            }

            return clone;
        }
    }
}
