using Pea.Core;
using Pea.Core.Entity;
using System.Collections.Generic;
using System.IO;

namespace PEA_VehicleScheduling_Example
{
    public class ResultWriter
    {
        public static void WriteResults(string fileName, IList<EntityBase> entities)
        {
            var trips = new List<Trip>();

            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    foreach (var entity in entities)
                    {
                        var vsEntity = entity as VehicleSchedulingEntity;
                        writer.WriteLine($"{vsEntity.VehiclesCount},{vsEntity.TotalDeadMileage},{vsEntity.TotalActiveTime}");
                    }
                }
            }
        }
    }
}
