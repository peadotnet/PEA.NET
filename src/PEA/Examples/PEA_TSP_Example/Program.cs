using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Pea.Configuration.ProblemModels;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;

namespace PEA_TSP_Example
{
    class Program
    {

        private static TSPEvaluation Evaluation;

        static void Main(string[] args)
        {
            var tspData = LoadCsv("Berlin52.csv");
            var initData = new TSPInitData(tspData);

            var system = PeaSystem.Create();
            system.Settings.AddSubProblem("TSP", new TravelingSalesmanProblem(tspData.Count));

            system.Settings.WithEntityType<TSPEntity>().WithEvaluation<TSPEvaluation>();

            var fitnessLimit = new MultiObjectiveFitness(1) { Value = { [0] = -7545 } };
            system.Settings.StopWhen().FitnessLimitExceeded(fitnessLimit)
                .Or().TimeoutElapsed(300000);

            Stopwatch sw = Stopwatch.StartNew();

            var result = system.Start(initData);
            //var result = AsyncUtil.RunSync(() => system.Start(initData));

            foreach (var reason in result.StopReasons)
            {
                Console.WriteLine(reason);
            }

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            var entities = TSPEvaluation.EntityCount;
            var speed = entities / elapsed;
            Console.WriteLine($"Elapsed: {elapsed} Entities: {entities} ({speed} ent./ms)");

            Console.ReadLine();

        }

        private static IList<IEntity> Evaluate(IList<IEntity> entitylist)
        {
            var result = new List<IEntity>();
            foreach (var entity in entitylist)
            {
                var entityWithKey = new Dictionary<MultiKey, IEntity>();
                entityWithKey.Add(TSPEvaluation.Key, entity);
                var decodedEntity = Evaluation.Decode(TSPEvaluation.Key, entityWithKey);
                result.Add(decodedEntity);
            }

            return result;
        }

        public static List<SpatialPoint> LoadCsv(string fileName)
        {
            var result = new List<SpatialPoint>();
            Assembly asm = Assembly.GetExecutingAssembly();
            var fullName = FormatResourceName(asm, fileName);
            Stream stream = asm.GetManifestResourceStream(fullName);
            StreamReader reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var fields = line.Split(",");
                var latitude = Convert.ToDouble(fields[0], CultureInfo.InvariantCulture);
                var longitude = Convert.ToDouble(fields[1], CultureInfo.InvariantCulture);
                result.Add(new SpatialPoint(latitude, longitude));
            }

            return result;
        }

        private static string FormatResourceName(Assembly assembly, string resourceName)
        {
            return assembly.GetName().Name + "." + resourceName.Replace(" ", "_")
                       .Replace("\\", ".")
                       .Replace("/", ".");
        }
    }
}
