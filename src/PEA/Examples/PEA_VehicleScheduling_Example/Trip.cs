namespace PEA_VehicleScheduling_Example
{
    public class Trip
    {
        public string TripId { get; }
        public string FirstStopId { get; }
        public int ArrivalTime { get; }
        public string LastStopId { get; }
        public int DepartureTime { get; }
        public string VehicleId { get; set; }
        public string CrewId { get; set; }

        public Trip(string tripId, string firstStopId, int arrivalTime, string lastStopId, int departureTime,
            string vehicleId = null, string crewId = null)
        {
            TripId = tripId;
            FirstStopId = firstStopId;
            ArrivalTime = arrivalTime;
            LastStopId = lastStopId;
            DepartureTime = departureTime;
            VehicleId = vehicleId;
            CrewId = crewId;
        }
    }
}
