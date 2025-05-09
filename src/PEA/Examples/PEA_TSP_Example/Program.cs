using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Pea;
using Pea.Configuration.ProblemModels;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.Restart;

namespace PEA_TSP_Example
{
	class Program : IExternalApp<TSPInitData>
    {

        private static TSPEvaluation Evaluation;

        static async Task Main(string[] args)
        {
            var tspData = LoadCsv("Berlin52.csv");
            var initData = new TSPInitData(tspData);

            Stopwatch sw = Stopwatch.StartNew();

            var instance = new Program();

            var result = await instance.StartAsync(initData);

            sw.Stop();

            foreach (var reason in result.StopReasons)
            {
                Console.WriteLine(reason);
            }

            var elapsed = sw.ElapsedMilliseconds;
            var entities = TSPEvaluation.EntityCount;
            var speed = entities / elapsed;
            var best = result.BestSolutions[0] as TSPEntity;
            Console.WriteLine($"Best distance: {best.TotalDistance}");
            Console.WriteLine($"Elapsed: {elapsed} Entities: {entities} ({speed} ent./ms)");

//            Console.ReadLine();

        }

        public async Task<PeaResult> StartAsync(TSPInitData initData)
        {
            var optimizer = Optimizer.Create();

            optimizer.Settings.WithRandom<FastRandom>().WithSeed(20250421); //(Environment.TickCount);
            optimizer.Settings.AddSubProblem("TSP", new TravelingSalesman(initData.TSPPoints.Count));
            optimizer.Settings.WithEntityType<TSPEntity>().WithEvaluation<TSPEvaluation>();
            optimizer.Settings.WithRestartStrategy(new UnchangedMeanRestartStrategy());
            optimizer.SetParameter("TestParameter", 42.0);

            var fitnessLimit = new MultiObjectiveFitness(new double[] { -7545 });
            optimizer.Settings.StopWhen().FitnessLimitExceeded(fitnessLimit)
                .Or().IterationsReached(100 * 1000 * 45);       //.TimeoutElapsed(300000);

            optimizer.SetParameter(Pea.Core.Island.ParameterNames.IslandsCount, 1);


            var result = await optimizer.Run(initData);
            //var result = AsyncUtil.RunSync(() => system.Start(initData));

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
