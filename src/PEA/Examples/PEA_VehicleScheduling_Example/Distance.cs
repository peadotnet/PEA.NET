namespace PEA_VehicleScheduling_Example
{
    public class Distance
    {
        public string Stop1Id { get; }
        public string Stop2Id { get; }
        public double DistanceKm { get; }
        public double Duration { get; }

        public Distance(string stop1Id, string stop2Id, double distanceKm, double duration)
        {
            Stop1Id = stop1Id.Trim();
            Stop2Id = stop2Id.Trim();
            DistanceKm = distanceKm;
            Duration = duration;
        }
    }
}
