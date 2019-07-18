using System;
using System.Collections.Generic;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VehicleSchedulingEntity : IEntity
    {
        public int IndexOfList { get; set; }
        public MultiKey OriginIslandKey { get; private set; }
        public IDictionary<string, IChromosome> Chromosomes { get; }
        public IFitness Fitness { get; set; }

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
