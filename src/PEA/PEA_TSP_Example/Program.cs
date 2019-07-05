using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Pea.Algorithm;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.Island;
using Pea.StopCriteria;

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

                .SetParameter(Pea.Algorithm.ParameterNames.MaxNumberOfEntities, 1000)
                .SetParameter(Pea.Algorithm.ParameterNames.MutationProbability, 0.9)
                .SetParameter(Pea.Chromosome.ParameterNames.ConflictReducingProbability, 0.5)
                .SetParameter(Pea.Chromosome.ParameterNames.FailedCrossoverRetryCount, 1)
                .SetParameter(Pea.Chromosome.ParameterNames.FailedMutationRetryCount, 2)
                .SetParameter(Pea.Selection.ParameterNames.TournamentSize, 4)
                .SetParameter(Pea.Island.ParameterNames.ArchipelagosCount, 1)
                .SetParameter(Pea.Island.ParameterNames.IslandsCount, 4)
                .SetParameter(Pea.Island.ParameterNames.PhenotypeDecodersCount, 2)
                .SetParameter(Pea.Island.ParameterNames.FitnessEvaluatorsCount, 2);

            system.Settings.Random = typeof(SystemRandom);

            var fitnessLimit = new MultiObjectiveFitness(1) { Value = { [0] = -7545 } };
            system.Settings.StopCriteria = StopCriteriaBuilder
                .StopWhen().FitnessLimitExceeded(fitnessLimit)
                .Or().TimeoutElapsed(300000)
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


            Task.Run(() => system.Start(initData)).GetAwaiter().GetResult();

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
