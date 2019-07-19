using System.Collections.Generic;

namespace PEA_VehicleScheduling_Example
{
    public class SortByArrivalThenDepartureComparer : IComparer<Trip>
    {
        public int Compare(Trip x, Trip y)
        {
            if (x.ArrivalTime != y.ArrivalTime) return x.ArrivalTime.CompareTo(y.ArrivalTime);

            if (x.DepartureTime != y.DepartureTime) return x.DepartureTime.CompareTo(y.DepartureTime);

            return 0;
        }
    }
}
