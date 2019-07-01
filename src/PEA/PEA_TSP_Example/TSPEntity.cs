using System.Collections.Generic;
using Pea.Core;

namespace PEA_TSP_Example
{
    public class TSPEntity : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; private set; }
        public IDictionary<string, IChromosome> Chromosomes { get; } = new Dictionary<string, IChromosome>();
        public List<SpatialPoint> Phenotype = new List<SpatialPoint>();
        public IFitness Fitness { get; set; }

        public object Clone()
        {
            var clone = new TSPEntity()
            {
                OriginIslandKey = this.OriginIslandKey
            };

            foreach (var chromosome in Chromosomes)
            {
                clone.Chromosomes.Add(chromosome.Key, chromosome.Value);
            }

            return clone;
        }
    }
}
