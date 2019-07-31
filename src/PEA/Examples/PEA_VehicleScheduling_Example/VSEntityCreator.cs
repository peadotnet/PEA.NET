using System.Collections.Generic;
using System.Linq;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VSEntityCreator : IEntityCreator
    {
        public static readonly string Key = "VehicleScheduling";

        public VSInitData InitData { get; private set; }

        public IConflictDetector ConflictDetector { get; private set; }

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
            IRandom random = new FastRandom();

            List<List<int>> indices = new List<List<int>>();

            for (int i = 0; i < InitData.Trips.Length; i++)
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

                if (!fitVehicles.Any())
                {
                    indices.Add(new List<int>());
                    indices[indices.Count-1].Add(i);
                }
                else
                {
                    var randomChoose = random.GetDouble(0, 1);
                    if (randomChoose < 0.99 || fitVehicles.Count == 1)
                    {
                        fitVehicles.Sort((x, y) => x.Key.CompareTo(y.Key));
                        indices[fitVehicles.First().Value].Add(i);
                    }
                    else
                    {
                        var randomIndex = random.GetInt(0, fitVehicles.Count);
                        indices[fitVehicles[randomIndex].Value].Add(i);
                    }
                }
            }

            var geneSections = new int[indices.Count][];
            for (int s = 0; s < indices.Count; s++)
            {
                geneSections[s] = indices[s].ToArray();
            }

            var chromosome = new SortedSubsetChromosome(geneSections);

            var entity = new VehicleSchedulingEntity();
            entity.Chromosomes.Add(Key, chromosome);

            return entity;
        }
    }
}
