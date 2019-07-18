using System.Collections.Generic;
using System.IO;

namespace PEA_VehicleScheduling_Example
{
    public static class DistanceLoader
    {
        public static List<Distance> LoadDistances(string fileName)
        {
            var distances = new List<Distance>();
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var fields = line.Split(',');
                        var distance = new Distance(fields[0], fields[1], double.Parse(fields[2]), double.Parse(fields[3]));
                        distances.Add(distance);
                    }
                }
            }

            return distances;
        }
    }
}
