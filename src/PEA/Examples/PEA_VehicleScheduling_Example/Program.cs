using Pea;
using Pea.Configuration.Scenarios;
using Pea.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PEA_VehicleScheduling_Example
{
    class Program
    {
        private static VSEvaluation Evaluation { get; set; }

        static void Main(string[] args)
        {
            var tripList = TripLoader.LoadTrips(@"Data/Trips_Szeged.csv");
            var distances = DistanceLoader.LoadDistances(@"Data/Distances_Szeged.csv");

            var initData = new VSInitData(tripList, distances);

            var optimizer = Optimizer.Create();
            optimizer.Settings.AddSubProblem("VehicleScheduling", new VehicleScheduling(tripList.Count))
                .AddConflictDetector<VSConflictDetector>();

            optimizer.Settings.WithEntityType<VehicleSchedulingEntity>()
                .WithEvaluation<VSEvaluation>();    //TODO: WithCreator

            optimizer.Settings.StopWhen().TimeoutElapsed(120 * 1000);

            optimizer.SetParameter(Pea.Engine.ParameterNames.IslandsCount, 1);



            //system.Settings.Random = typeof(FastRandom);  //-> PeaSettings


            //Evaluation = new VSEvaluation();
            //Evaluation.Init(initData);

            //var creator = new VSEntityCreator();
            //creator.Init(initData);

            //var conflictDetector = new VSConflictDetector();
            //conflictDetector.Init(initData);

            //Chromosome.Implementation.SortedSubset.SortedSubsetChromosomeValidator.ConflictDetector = conflictDetector;

            //var islandEngine = Island.IslandEngineFactory.Create(system.Settings.Build());
            ////islandEngine.EntityCreators.Add(creator, 1);

            //var algorithmFactory = new Algorithm.SteadyState();
            //var algorithm = algorithmFactory.GetAlgorithm(islandEngine);
            //islandEngine.Algorithm = algorithm;


            Stopwatch sw = Stopwatch.StartNew();


            var result = optimizer.Run(initData).GetAwaiter().GetResult();

            foreach (var reason in result.StopReasons)
            {
                Console.WriteLine(reason);
            }

            //try
            //{
            //    initData.Build();
            //    islandEngine.InitConflictDetectors(initData);
            //    algorithm.InitPopulation();
            //    var c = 0;
            //    while (true)
            //    {
            //        algorithm.RunOnce();
            //        var stopDecision = islandEngine.StopCriteria.MakeDecision(islandEngine, algorithm.Population);
            //        if (stopDecision.MustStop)
            //        {
            //            Console.WriteLine(stopDecision.Reasons[0]);
            //            break;
            //        }

            //        c++;
            //    }
            //}
            //catch (ApplicationException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            var entities = VSEvaluation.EntityCount;
            var speed = entities / (double)elapsed;
            Console.WriteLine($"Elapsed: {elapsed} Entities: {entities} ({speed} ent./ms)");

            //ResultWriter.WriteResults("VehicleSchedulingResults", algorithm.Population.Bests);

            Console.ReadLine();


        }

        private static IList<EntityBase> Evaluate(IList<EntityBase> entitylist)
        {
            var result = new List<EntityBase>();
            foreach (var entity in entitylist)
            {
                var entityWithKey = new Dictionary<MultiKey, EntityBase>();
                entityWithKey.Add(VSEvaluation.Key, entity);
                var decodedEntity = Evaluation.Decode(VSEvaluation.Key, entityWithKey);
                if (decodedEntity != null) result.Add(decodedEntity);
            }

            return result;
        }
    }
}
