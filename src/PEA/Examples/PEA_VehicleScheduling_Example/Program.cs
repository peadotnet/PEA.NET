using System;

namespace PEA_VehicleScheduling_Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var trips = TripLoader.LoadTrips("Trips_Szeged.csv");
            var distances = DistanceLoader.LoadDistances("Distances_Szeged.csv");


        }
    }
}
