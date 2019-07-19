using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;

namespace PEA_VehicleScheduling_Example
{
    public class VSEvaluation : IEvaluation
    {
        public static readonly MultiKey Key = new MultiKey("VehicleScheduling");

        public static int EntityCount = 0;

        public VSInitData InitData { get; private set; }

        public IConflictDetector ConflictDetector { get; private set; }

        public void Init(IEvaluationInitData initData)
        {
            InitData = (VSInitData)initData;
            InitData.Build();
            ConflictDetector = new VSConflictDetector();
            ConflictDetector.Init(initData);
        }

        public IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            EntityCount++;

            bool hardConflict = false;
            var entity = entities[Key] as VehicleSchedulingEntity;
            var chromosome = entity.Chromosomes[Key[0]] as SortedSubsetChromosome;

            entity.VehiclesCount = chromosome.Sections.Length;
            entity.CrewCount = chromosome.Sections.Length;

            for (int s = 0; s < chromosome.Sections.Length; s++)
            {
                for (int p = 0; p < chromosome.Sections[s].Length-1; p++)
                {
                    if (ConflictDetector.ConflictDetected(chromosome.Sections[s][p], chromosome.Sections[s][p + 1]))
                    {
                        return null;
                    }

                    var trip1 = InitData.Trips[chromosome.Sections[s][p]];
                    var trip2 = InitData.Trips[chromosome.Sections[s][p + 1]];

                    double duration = InitData.GetDuration(trip1.LastStopId, trip2.FirstStopId);
                    double distance = InitData.GetDistance(trip1.LastStopId, trip2.FirstStopId);

                    if (trip1.DepartureTime + duration > trip2.ArrivalTime)
                    {
                        hardConflict = true;
                    }

                    entity.TotalDeadMileage += distance;
                }
            }

            if (hardConflict) return null;

            MultiObjectiveFitness fitness = new MultiObjectiveFitness(3);
            fitness.Value[0] = 1 /(1 + entity.TotalDeadMileage);
            fitness.Value[1] = 1 / (1 + (double)entity.VehiclesCount);
            fitness.Value[2] = 1 / (1 + (double)entity.CrewCount);

            entity.Fitness = fitness;

            return entity;
        }

        public IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new List<IEntity>() { entities[Key] };
        }
    }
}
