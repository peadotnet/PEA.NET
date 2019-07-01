using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Pea.Algorithm;
using Pea.Core;
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
                .WithAlgorithm<Pea.Algorithm.SteadyState>()
                .AddChromosome<Pea.Chromosome.Permutation>("TSP")
                .WithFitness<Pea.Fitness.ParetoMultiobjective>()
                .AddSelection<Pea.Selection.TournamentSelection>()
                .AddReinsertion<Pea.Reinsertion.ReplaceParentsReinsertion>()
                .WithCreator<TSPEntityCreator>();

            system.Settings.Random = typeof(SystemRandom);

            var evaluator = new TSPEvaluation();
            evaluator.Init(initData);

            var creator = new TSPEntityCreator();
            creator.Init(initData);

            var islandEngine = IslandEngineFactory.Create(system.Settings);
            islandEngine.EntityCreators.Add(creator, 1);

            var algorithmFactory = new SteadyState();
            var algorithm = algorithmFactory.GetAlgorithm(islandEngine, Evaluate);
            islandEngine.Algorithm = algorithm;

            islandEngine.StopCriteria = StopCriteriaBuilder.StopWhen().TimeoutElapsed(10000).Build();
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
