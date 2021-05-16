using Pea.Core;
using Pea.Core.Entity;

namespace PEA_VehicleScheduling_Example
{
	public class VehicleSchedulingEntity : EntityBase
    {
        public int VehiclesCount = 0;
        public double TotalDeadMileage = 0;
        public int TotalActiveTime = 0;

        public override IEntity Clone(bool cloneChromosomes)
        {
            var clone = base.Clone(cloneChromosomes) as VehicleSchedulingEntity;
            clone.OriginIslandKey = this.OriginIslandKey;
            return clone;
        }

        public override string ToString()
        {
            return $"Vehicles: {VehiclesCount} Dead mileage: {TotalDeadMileage} Active time: {TotalActiveTime/60}:{TotalActiveTime%60}";
        }
    }
}
