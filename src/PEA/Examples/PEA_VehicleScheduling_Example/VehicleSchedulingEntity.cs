using Pea.Core.Entity;

namespace PEA_VehicleScheduling_Example
{
	public class VehicleSchedulingEntity : EntityBase
    {
        public int VehiclesCount = 0;
        public double TotalDeadMileage = 0;
        public int TotalActiveTime = 0;

        public override object Clone()
        {
            var clone = base.Clone() as VehicleSchedulingEntity;
            clone.OriginIslandKey = this.OriginIslandKey;
            return clone;
        }

        public override string ToString()
        {
            return $"Vehicles: {VehiclesCount} Dead mileage: {TotalDeadMileage} Active time: {TotalActiveTime/60}:{TotalActiveTime%60}";
        }
    }
}
