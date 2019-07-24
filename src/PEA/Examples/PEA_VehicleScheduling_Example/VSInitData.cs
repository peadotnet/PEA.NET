using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VSInitData : IEvaluationInitData
    {
        public IList<Trip> TripList { get; }
        public IList<Distance> Distances { get; }

        public bool AlreadyBuilt = false;

        public Dictionary<string, int> StopIds = new Dictionary<string, int>();
        public Trip[] Trips { get; private set; }

        public double[,] DurationMatrix;
        public double[,] DistanceMatrix;

        public VSInitData(IList<Trip> tripList, IList<Distance> distances)
        {
            TripList = tripList;
            Distances = distances;
        }

        public void Build()
        {
            if (AlreadyBuilt) return;

            Trips = TripList.ToArray();
            Array.Sort<Trip>(Trips, new SortByArrivalThenDepartureComparer());

            int stopsCounter = 0;
            foreach (var trip in TripList)
            {
                if (!StopIds.ContainsKey(trip.FirstStopId))
                {
                    StopIds.Add(trip.FirstStopId, stopsCounter);
                    stopsCounter++;
                }

                if (!StopIds.ContainsKey(trip.LastStopId))
                {
                    StopIds.Add(trip.LastStopId, stopsCounter);
                    stopsCounter++;
                }
            }

            DurationMatrix = new double[stopsCounter, stopsCounter];
            DistanceMatrix = new double[stopsCounter, stopsCounter];

            foreach (var distance in Distances)
            {
                if (!StopIds.ContainsKey(distance.Stop1Id) || !StopIds.ContainsKey(distance.Stop2Id))
                {
                    continue;
                }

                var id1 = StopIds[distance.Stop1Id];
                var id2 = StopIds[distance.Stop2Id];

                DurationMatrix[id1, id2] = distance.Duration;
                DistanceMatrix[id1, id2] = distance.DistanceKm;
            }

            AlreadyBuilt = true;
        }

        public double GetDuration(string stopId1, string stopId2)
        {
            if (!StopIds.ContainsKey(stopId1))
            {
                Console.WriteLine("Stop ID not found in distance matrix: " + stopId1);
                return 0;
            }

            if (!StopIds.ContainsKey(stopId2))
            {
                Console.WriteLine("Stop ID not found in distance matrix: " + stopId2);
                return 0;
            }

            return DurationMatrix[StopIds[stopId1], StopIds[stopId2]];
        }

        public double GetDistance(string stopId1, string stopId2)
        {
            if (!StopIds.ContainsKey(stopId1))
            {
                return 0;
            }

            if (!StopIds.ContainsKey(stopId2))
            {
                return 0;
            }

            return DistanceMatrix[StopIds[stopId1], StopIds[stopId2]];
        }
    }
}
