using System;
using System.Collections.Generic;
using System.IO;

namespace PEA_VehicleScheduling_Example
{
    public static class TripLoader
    {
        public static List<Trip> LoadTrips(string fileName)
        {
            var trips = new List<Trip>();

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var fields = line.Split(',');
                        var trip = new Trip(fields[0], fields[1], Int32.Parse(fields[2]), fields[3], Int32.Parse(fields[4]), fields[5], fields[6]);
                        trips.Add(trip);
                    }
                }
            }

            return trips;
        }
    }
}
