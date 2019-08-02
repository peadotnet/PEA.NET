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

            int StopsCounter = 0;
            foreach (var trip in Trips)
            {
                if (!StopIds.ContainsKey(trip.FirstStopId))
                {
                    StopIds.Add(trip.FirstStopId, StopsCounter);
                    StopsCounter++;
                }

                if (!StopIds.ContainsKey(trip.LastStopId))
                {
                    StopIds.Add(trip.LastStopId, StopsCounter);
                    StopsCounter++;
                }
            }

            DurationMatrix = new double[StopsCounter, StopsCounter];
            DistanceMatrix = new double[StopsCounter, StopsCounter];

            FillMatrixWithInitValues(StopsCounter);

            foreach (var distance in Distances)
            {
                if (!StopIds.ContainsKey(distance.Stop1Id) || !StopIds.ContainsKey(distance.Stop2Id)) continue;

                var id1 = StopIds[distance.Stop1Id];
                var id2 = StopIds[distance.Stop2Id];

                if (DurationMatrix[id1, id2] > distance.Duration)
                {
                    DurationMatrix[id1, id2] = distance.Duration;
                }

                DistanceMatrix[id1, id2] = distance.DistanceKm;
            }

            foreach (var trip in Trips)
            {
                if (!StopIds.ContainsKey(trip.FirstStopId) || !StopIds.ContainsKey(trip.LastStopId)) continue;

                var id1 = StopIds[trip.FirstStopId];
                var id2 = StopIds[trip.LastStopId];

                if (DurationMatrix[id1, id2] > trip.DepartureTime - trip.ArrivalTime)
                {
                    DurationMatrix[id1, id2] = trip.DepartureTime - trip.ArrivalTime;
                }
            }

            for (int k = 0; k < StopsCounter; k++)
            {
                for (int i = 0; i < StopsCounter; i++)
                {
                    for (int j = 0; j < StopsCounter; j++)
                    {
                        if (DurationMatrix[i, j] > DurationMatrix[i, k] + DurationMatrix[k, j])
                        {
                            DurationMatrix[i, j] = DurationMatrix[i, k] + DurationMatrix[k, j];
                        }
                    }
                }
            }

            AlreadyBuilt = true;
        }

        private void FillMatrixWithInitValues(int StopsCounter)
        {
            for (int i = 0; i < StopsCounter; i++)
            {
                for (int j = 0; j < StopsCounter; j++)
                {
                    if (i != j) DurationMatrix[i, j] = double.MaxValue;
                }
            }
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
