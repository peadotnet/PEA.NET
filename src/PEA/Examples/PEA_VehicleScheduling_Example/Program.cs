using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pea.Configuration.ProblemModels;
using Pea.Core;
using Pea.StopCriteria;
using Algorithm = Pea.Algorithm;
using Island = Pea.Core.Island;
using Chromosome = Pea.Chromosome;

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

            var system = PeaSystem.Create();
            system.Settings.AddSubProblem("VehicleScheduling", new VehicleSchedulingProblem(tripList.Count))
                .AddConflictDetector<VSConflictDetector>();

            system.Settings.WithEntityType<VehicleSchedulingEntity>()
                .WithEvaluation<VSEvaluation>();    //TODO: WithCreator

            system.Settings.StopWhen().TimeoutElapsed(120 * 1000);



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


            var result = system.Start(initData);
            //var result = AsyncUtil.RunSync(() => system.Start(initData));

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

        private static IList<IEntity> Evaluate(IList<IEntity> entitylist)
        {
            var result = new List<IEntity>();
            foreach (var entity in entitylist)
            {
                var entityWithKey = new Dictionary<MultiKey, IEntity>();
                entityWithKey.Add(VSEvaluation.Key, entity);
                var decodedEntity = Evaluation.Decode(VSEvaluation.Key, entityWithKey);
                if (decodedEntity != null) result.Add(decodedEntity);
            }

            return result;
        }
    }
}
