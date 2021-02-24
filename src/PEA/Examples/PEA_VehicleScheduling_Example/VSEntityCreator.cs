using System;
using System.Collections.Generic;
using System.Linq;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VSEntityCreator : IEntityCreator
    {
        public static readonly string Key = "VehicleScheduling";

        public double GreedyProbability { get; set; } = 0.99;

        public VSInitData InitData { get; private set; }

        public INeighborhoodConflictDetector ConflictDetector { get; private set; }

        //TODO: abstract constructor with conflictdetector

        public void Init(IEvaluationInitData initData)
        {
            InitData = (VSInitData)initData;
            InitData.Build();
            ConflictDetector = new VSConflictDetector();
            ConflictDetector.Init(initData);
        }

        public IEntity CreateEntity()
        {
            IRandom random = new FastRandom(DateTime.Now.Millisecond);

            List<List<int>> indices = new List<List<int>>();

            for (int i = 0; i < InitData.Trips.Length; i++)
            {

                int targetIndex;

                targetIndex = ChooseTarget(random, indices, i);

                if (targetIndex == indices.Count)
                {
                    indices.Add(new List<int>());
                }
                indices[targetIndex].Add(i);
            }

            int[][] geneSections = CreateSectionsFromList(indices);

            var chromosome = new SortedSubsetChromosome(geneSections);

            var entity = new VehicleSchedulingEntity();
            entity.Chromosomes.Add(Key, chromosome);

            return entity;
        }

        private int ChooseTarget(IRandom random, List<List<int>> indices, int i)
        {
            int targetIndex;
            var fitVehicles = CheckFitVehicles(indices, i);

            if (!fitVehicles.Any())
            {
                targetIndex = indices.Count;
            }
            else
            {
                targetIndex = ChooseFromFitVehicles(random, fitVehicles, GreedyProbability);
            }

            return targetIndex;
        }

        private List<KeyValuePair<double, int>> CheckFitVehicles(List<List<int>> indices, int i)
        {
            var fitVehicles = new List<KeyValuePair<double, int>>();

            for (int s = 0; s < indices.Count; s++)
            {
                var previousIndex = indices[s][indices[s].Count - 1];

                if (!ConflictDetector.ConflictDetected(previousIndex, i))
                {
                    var trip1 = InitData.Trips[previousIndex];
                    var trip2 = InitData.Trips[i];
                    var distance = InitData.GetDistance(trip1.LastStopId, trip2.FirstStopId);
                    fitVehicles.Add(new KeyValuePair<double, int>(distance, s));
                }
            }

            return fitVehicles;
        }

        private static int ChooseFromFitVehicles(IRandom random, List<KeyValuePair<double, int>> fitVehicles, double greedyProbability)
        {
            int targetIndex;
            var randomChoose = random.GetDouble(0, 1);
            if (randomChoose < greedyProbability || fitVehicles.Count == 1)
            {
                fitVehicles.Sort((x, y) => x.Key.CompareTo(y.Key));
                targetIndex = fitVehicles.First().Value;

            }
            else
            {
                var randomIndex = random.GetInt(0, fitVehicles.Count);
                targetIndex = fitVehicles[randomIndex].Value;
            }

            return targetIndex;
        }

        private static int[][] CreateSectionsFromList(List<List<int>> indices)
        {
            var geneSections = new int[indices.Count][];
            for (int s = 0; s < indices.Count; s++)
            {
                geneSections[s] = indices[s].ToArray();
            }

            return geneSections;
        }

    }
}
