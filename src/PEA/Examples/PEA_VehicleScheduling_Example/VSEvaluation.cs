using System;
using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.Util;

namespace PEA_VehicleScheduling_Example
{
    public class VSEvaluation : EvaluationBase
    {
        public static readonly MultiKey Key = new MultiKey("VehicleScheduling");

        public static int EntityCount = 0;

        public VSInitData InitData { get; private set; }

        public INeighborhoodConflictDetector ConflictDetector { get; private set; }

        //TODO: abstract constructor with conflictdetector

        public VSEvaluation(ParameterSet parameterSet) : base(parameterSet) { }

        public override void Init(IEvaluationInitData initData)
        {
            InitData = (VSInitData)initData;
            InitData.Build();
            ConflictDetector = new VSConflictDetector();
            ConflictDetector.Init(initData);
        }

        public override IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            EntityCount++;
            SortedSubsetChromosomeValidator.EntityCount = EntityCount;

            var entity = entities[Key] as VehicleSchedulingEntity;
            var chromosome = entity.Chromosomes[Key[0]] as SortedSubsetChromosome;

            entity.VehiclesCount = chromosome.Sections.Length;

            for (int s = 0; s < chromosome.Sections.Length; s++)
            {
                //int DailyFirstTime = InitData.Trips[chromosome.Sections[s][0]].ArrivalTime;
                //int length = chromosome.Sections[s].Length;
                //int DailyLastTime = InitData.Trips[chromosome.Sections[s][length-1]].DepartureTime;
                //int? AMLastTime = null;
                //int? PMFirstTime = null;
                
                for (int p = 0; p < chromosome.Sections[s].Length-1; p++)
                {
                    if (ConflictDetector.ConflictDetected(chromosome.Sections[s][p], chromosome.Sections[s][p + 1]))
                    {
                        throw new ApplicationException("Shit happend");
                        //return null;    //hard conflict
                    }

                    var trip1 = InitData.Trips[chromosome.Sections[s][p]];
                    var trip2 = InitData.Trips[chromosome.Sections[s][p + 1]];

                    double distance = InitData.GetDistance(trip1.LastStopId, trip2.FirstStopId);
                    entity.TotalDeadMileage += distance;

                    //if ((trip1.DepartureTime <= 720) && (!AMLastTime.HasValue || trip1.DepartureTime > AMLastTime)) AMLastTime = trip1.DepartureTime;
                    //if ((trip1.ArrivalTime >= 720) && (!PMFirstTime.HasValue || trip1.ArrivalTime < PMFirstTime)) PMFirstTime = trip1.ArrivalTime;

                }

                //var lastTrip = InitData.Trips[chromosome.Sections[s][length-1]];
                //if ((lastTrip.DepartureTime <= 720) && (!AMLastTime.HasValue || lastTrip.DepartureTime > AMLastTime)) AMLastTime = lastTrip.DepartureTime;
                //if ((lastTrip.ArrivalTime >= 720) && (!PMFirstTime.HasValue || lastTrip.ArrivalTime < PMFirstTime)) PMFirstTime = lastTrip.ArrivalTime;

                //entity.TotalActiveTime += DailyLastTime - DailyFirstTime;
                //if (AMLastTime.HasValue) entity.TotalActiveTime -= (720 - AMLastTime.Value);
                //if (PMFirstTime.HasValue) entity.TotalActiveTime -= (PMFirstTime.Value - 720);
            }

            var fitness0 = 1 / (1 + entity.TotalDeadMileage);
            var fitness1 = 1 / (1 + (double)entity.VehiclesCount);
            MultiObjectiveFitness fitness = new MultiObjectiveFitness(new double[] { fitness0, fitness1 });
            
            //fitness.Value[2] = 1 / (1 + (double) entity.TotalActiveTime);
            //GetAverageLengthOfLongSections(chromosome.Sections);

            entity.SetFitness(fitness);

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

        public override IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
        {
            return new List<IEntity>() { entities[Key] };
        }
    }
}
