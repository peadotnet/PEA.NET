using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var system = PeaSystem.Create()
                .WithAlgorithm<Algorithm.SteadyState>()
                .AddChromosome<Pea.Chromosome.SortedSubset>("VehicleScheduling")
                .WithConflictDetector<VSConflictDetector>()
                .WithCreator<VSEntityCreator>()
                .WithEvaluation<VSEvaluation>()

                .WithFitness<Pea.Fitness.ParetoMultiobjective>()
                .AddSelection<Pea.Selection.TournamentSelection>()
                .AddReinsertion<Pea.Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion>()

                .SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, 2500)
                .SetParameter(Algorithm.ParameterNames.MutationProbability, 0.5)
                .SetParameter(Pea.Selection.ParameterNames.TournamentSize, 2)
                .SetParameter(Island.ParameterNames.ArchipelagosCount, 1)
                .SetParameter(Island.ParameterNames.IslandsCount, 1)
                .SetParameter(Island.ParameterNames.EvaluatorsCount, 2)
                .SetParameter(Chromosome.ParameterNames.ConflictReducingProbability, 0.5)
                .SetParameter(Chromosome.ParameterNames.FailedCrossoverRetryCount, 20)
                .SetParameter(Chromosome.ParameterNames.FailedMutationRetryCount, 10);
                
            system.Settings.Random = typeof(FastRandom);

            system.Settings.StopCriteria = StopCriteriaBuilder
                .StopWhen().TimeoutElapsed(3600 * 1000)
                .Build();

            Evaluation = new VSEvaluation();
            Evaluation.Init(initData);

            var creator = new VSEntityCreator();
            creator.Init(initData);

            var conflictDetector = new VSConflictDetector();
            conflictDetector.Init(initData);
            Chromosome.Implementation.SortedSubset.SortedSubsetChromosomeValidator.ConflictDetector = conflictDetector;

            var islandEngine = Island.IslandEngineFactory.Create(system.Settings);
            islandEngine.EntityCreators.Add(creator, 1);

            var algorithmFactory = new Algorithm.SteadyState();
            var algorithm = algorithmFactory.GetAlgorithm(islandEngine, Evaluate);
            islandEngine.Algorithm = algorithm;


            Stopwatch sw = Stopwatch.StartNew();

            //system.Start(initData).GetAwaiter().GetResult();

            //var result = AsyncUtil.RunSync(() => system.Start(initData));

            //foreach (var reason in result.StopReasons)
            //{
            //    Console.WriteLine(reason);
            //}

            try
            {

                initData.Build();
                islandEngine.ConflictDetector.Init(initData);
                algorithm.InitPopulation();
                var c = 0;
                while (true)
                {
                    algorithm.RunOnce();
                    var stopDecision = islandEngine.StopCriteria.MakeDecision(islandEngine, algorithm.Population);
                    if (stopDecision.MustStop)
                    {
                        Console.WriteLine(stopDecision.Reasons[0]);
                        break;
                    }

                    c++;
                }
            }
            catch (ApplicationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            var entities = VSEvaluation.EntityCount;
            var speed = entities / (double)elapsed;
            Console.WriteLine($"Elapsed: {elapsed} Entities: {entities} ({speed} ent./ms)");

            ResultWriter.WriteResults("VehicleSchedulingResults", algorithm.Population.Bests);

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
