namespace PEA_TSP_Example
{
    public class SpatialPoint
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public SpatialPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
