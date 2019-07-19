using System.Collections.Generic;
using System.Globalization;
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
                    var line = reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        var fields = line.Split(',');
                        var distance = new Distance(fields[0], fields[1], Parse(fields[2]), Parse(fields[3]));
                        distances.Add(distance);
                    }
                }
            }

            return distances;
        }

        public static double Parse(string valueString)
        {
            return double.Parse(valueString, CultureInfo.InvariantCulture);
        }
    }
}
