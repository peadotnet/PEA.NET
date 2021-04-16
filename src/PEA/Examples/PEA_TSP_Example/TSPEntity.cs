using System.Collections.Generic;
using System.Globalization;
using Pea.Core.Entity;

namespace PEA_TSP_Example
{
	public class TSPEntity : EntityBase
    {
        public List<SpatialPoint> Phenotype = new List<SpatialPoint>();

        public double TotalDistance { get; set; }

        public override object Clone()
        {
            var clone = base.Clone() as TSPEntity;
            clone.OriginIslandKey = this.OriginIslandKey;
            return clone;
        }

        public override string ToString()
        {
            return "Distance: " + TotalDistance.ToString(CultureInfo.InvariantCulture);
        }
    }
}
