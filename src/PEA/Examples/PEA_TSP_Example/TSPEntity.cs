using System.Collections.Generic;
using System.Globalization;
using Pea.Core;
using Pea.Core.Entity;

namespace PEA_TSP_Example
{
	public class TSPEntity : EntityBase
    {
        public List<SpatialPoint> Phenotype = new List<SpatialPoint>();

        public double TotalDistance { get; set; }

        public override IEntity Clone(bool cloneChromosomes)
        {
            var clone = base.Clone(cloneChromosomes) as TSPEntity;
            clone.OriginIslandKey = this.OriginIslandKey;
            return clone;
        }

        public override string ToString()
        {
            return "Distance: " + TotalDistance.ToString(CultureInfo.InvariantCulture);
        }
    }
}
