using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using Pea.Algorithm;
using Pea.Core;
using Pea.Core.Island;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.StopCriteria;
using ParameterNames = Pea.Core.Island.ParameterNames;

namespace PEA_TSP_Example
{
    class Program
    {

        private static TSPEvaluation Evaluation;

        static void Main(string[] args)
        {
            var tspData = LoadCsv("Berlin52.csv");
            var initData = new TSPInitData(tspData);

            var system = PeaSystem.Create()
                .WithAlgorithm<SteadyState>()
                .AddChromosome<Pea.Chromosome.Permutation>("TSP")
                .WithFitness<Pea.Fitness.ParetoMultiobjective>()
                .AddSelection<Pea.Selection.TournamentSelection>()
                .AddReinsertion<Pea.Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion>()
                .WithCreator<TSPEntityCreator>()
                .WithEvaluation<TSPEvaluation>()

                .SetParameter(Pea.Algorithm.ParameterNames.MaxNumberOfEntities, 500)
                .SetParameter(Pea.Algorithm.ParameterNames.MutationProbability, 0.7)
                .SetParameter(Pea.Selection.ParameterNames.TournamentSize, 2)
                .SetParameter(ParameterNames.ArchipelagosCount, 1)
                .SetParameter(ParameterNames.IslandsCount, 1)
                .SetParameter(ParameterNames.EvaluatorsCount, 2);

            system.Settings.Random = typeof(FastRandom);

            var fitnessLimit = new MultiObjectiveFitness(1) { Value = { [0] = -7545 } };
            system.Settings.StopCriteria = StopCriteriaBuilder
                .StopWhen().FitnessLimitExceeded(fitnessLimit)
                //.Or().TimeoutElapsed(180000)
                .Build();

            Evaluation = new TSPEvaluation();
            Evaluation.Init(initData);

            var creator = new TSPEntityCreator();
            creator.Init(initData);

            var islandEngine = IslandEngineFactory.Create(system.Settings);
            islandEngine.EntityCreators.Add(creator, 1);

            var algorithmFactory = new SteadyState();
            var algorithm = algorithmFactory.GetAlgorithm(islandEngine, Evaluate);
            islandEngine.Algorithm = algorithm;


            Stopwatch sw = Stopwatch.StartNew();

            system.Start(initData).GetAwaiter().GetResult();

            var result = AsyncUtil.RunSync(() => system.Start(initData));

            foreach (var reason in result.StopReasons)
            {
                Console.WriteLine(reason);
            }

            //algorithm.InitPopulation();
            //var c = 0;
            //while (true)
            //{
            //    algorithm.RunOnce();
            //    var stopDecision = islandEngine.StopCriteria.MakeDecision(islandEngine, algorithm.Population);
            //    if (stopDecision.MustStop)
            //    {
            //        Console.WriteLine(stopDecision.Reasons[0]);
            //        break;
            //    }
            //    c++;
            //}

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
