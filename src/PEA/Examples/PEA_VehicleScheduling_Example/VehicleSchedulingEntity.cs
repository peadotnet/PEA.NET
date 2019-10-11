using System.Collections.Generic;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VehicleSchedulingEntity : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; private set; }
        public IDictionary<string, IChromosome> Chromosomes { get; set; } = new Dictionary<string, IChromosome>();
        public IFitness Fitness { get; set; }
        public Dictionary<string, string> LastCrossOvers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LastMutations { get; set; } = new Dictionary<string, string>();

        public int VehiclesCount = 0;
        public double TotalDeadMileage = 0;
        public int TotalActiveTime = 0;

        public object Clone()
        {
            var clone = new VehicleSchedulingEntity()
            {
                OriginIslandKey = this.OriginIslandKey
            };

            foreach (var chromosome in Chromosomes)
            {
                clone.Chromosomes.Add(chromosome.Key, chromosome.Value);
            }

            return clone;
        }

        public override string ToString()
        {
            return $"Vehicles: {VehiclesCount} Dead mileage: {TotalDeadMileage} Active time: {TotalActiveTime/60}:{TotalActiveTime%60}";
        }
    }
}
