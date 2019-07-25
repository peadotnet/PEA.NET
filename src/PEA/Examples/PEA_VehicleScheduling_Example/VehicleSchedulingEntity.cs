using System;
using System.Collections.Generic;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VehicleSchedulingEntity : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; private set; }
        public IDictionary<string, IChromosome> Chromosomes { get; } = new Dictionary<string, IChromosome>();
        public IFitness Fitness { get; set; }
        public Dictionary<string, string> LastCrossOvers { get; } = new Dictionary<string, string>();
        public Dictionary<string, string> LastMutations { get; } = new Dictionary<string, string>();

        public int VehiclesCount = 0;
        public int CrewCount = 0;
        public double TotalDeadMileage = 0;

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
            return $"Vehicles: {VehiclesCount} Dead mileage: {TotalDeadMileage}";
        }
    }
}
