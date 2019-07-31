using System;
using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.Util;

namespace PEA_VehicleScheduling_Example
{
    public class VSEvaluation : IEvaluation
    {
        public static readonly MultiKey Key = new MultiKey("VehicleScheduling");

        public static int EntityCount = 0;

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

        public IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            EntityCount++;
            SortedSubsetChromosomeValidator.EntityCount = EntityCount;

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
                        return null;    //hard conflict
                    }

                    var trip1 = InitData.Trips[chromosome.Sections[s][p]];
                    var trip2 = InitData.Trips[chromosome.Sections[s][p + 1]];

                    //double duration = InitData.GetDuration(trip1.LastStopId, trip2.FirstStopId);
                    double distance = InitData.GetDistance(trip1.LastStopId, trip2.FirstStopId);

                    //if (trip1.DepartureTime + duration > trip2.ArrivalTime)
                    //{
                    //    hardConflict = true;
                    //}

                    entity.TotalDeadMileage += distance;
                }
            }

            MultiObjectiveFitness fitness = new MultiObjectiveFitness(2);
            fitness.Value[0] = 1 /(1 + entity.TotalDeadMileage);
            fitness.Value[1] = 1 / (1 + (double)entity.VehiclesCount);
            //fitness.Value[2] =
            GetAverageLengthOfLongSections(chromosome.Sections);

            entity.Fitness = fitness;

            return entity;
        }


        public double GetAverageLengthOfLongSections(int[][] sections)
        {
            var sectionsCount = sections.Length;

            var comparer = new ArrayLengthComparer();
            var sorter = new QuickSorter<int[]>();
            sorter.Sort(sections, comparer, 0, sectionsCount-1);

            var totalLength = 0;
            var longSectionsCount = sectionsCount * 3/4;
            for (int i = 0; i < longSectionsCount; i++)
            {
                totalLength += sections[i].Length;
            }

            return totalLength/(double)longSectionsCount;
        }

        public IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new List<IEntity>() { entities[Key] };
        }
    }
}
