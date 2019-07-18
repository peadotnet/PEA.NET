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
                
                var fitVehicles = new List<int>();

                for (int s = 0; s < indices.Count; s++)
                {
                    var previousIndex = indices[s][indices[s].Count - 1];

                    if (!ConflictDetector.ConflictDetected(i, previousIndex))
                    {
                        fitVehicles.Add(s);
                    }
                }

                if (!fitVehicles.Any())
                {
                    indices.Add(new List<int>());
                    indices[indices.Count-1].Add(i);
                }
                else
                {
                    var randomIndex = random.GetInt(0, fitVehicles.Count);
                    indices[fitVehicles[randomIndex]].Add(i);
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
