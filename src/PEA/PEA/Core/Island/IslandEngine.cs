using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pea.Migration;

namespace Pea.Core.Island
{
    public class IslandEngine : IEngine
    {
        public IAlgorithm Algorithm { get; set; }

        public IRandom Random { get; set; }

        public Configuration.Implementation.PeaSettings Settings { get; set; }
        public ParameterSet Parameters { get; set; }
        public IDictionary<string, IList<IConflictDetector>> ConflictDetectors { get; set; }
        public IProvider<IEntityCreator> EntityCreators { get; set; }
        public IProvider<ISelection> Selections { get; set; }
        public IReduction Reduction { get; set; }
        public IFitnessComparer FitnessComparer { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public IProvider<IReplacement> Replacements { get; set; }
        public IStopCriteria StopCriteria { get; set; }
        public EvaluationBase Evaluation { get; set; }
        public IMigrationStrategy MigrationStrategy { get; set; }
        public LaunchTravelersDelegate LaunchTravelers { get; set; }

        public NewEntitiesMergedToBestDelegate NewEntityMergedToBest { get; set; }

        public IslandEngine()
        {
             
        }

		public void Init(IEvaluationInitData initData)
        {
            initData.Build();
            InitConflictDetectors(initData);
            InitEntityCreators(initData);
            Algorithm.InitPopulation();
        }

		private void InitEntityCreators(IEvaluationInitData initData)
		{
			foreach (var creator in EntityCreators)
			{
                creator.Init(initData);
			}
		}

		public void InitConflictDetectors(IEvaluationInitData initData)
        {
            foreach (var key in ConflictDetectors.Keys)
            {
                foreach (var detector in ConflictDetectors[key])
                {
                    detector.Init(initData);
                }
            }
        }

        public StopDecision RunOnce()
        {
            Algorithm.RunOnce();
            return StopCriteria.MakeDecision(this, Algorithm.Population);
        }

        public void Reduct()
		{
            Debug.WriteLine("Reduction of population...");
            Reduction.Reduct(Algorithm.Population);
            var timeString = DateTime.Now.ToString("HH:mm:ss.ffff");
            var stat = Algorithm.Population.FitnessStatistics.StatisticVariables[0];
            Debug.WriteLine($"{timeString} Mean: {stat.Mean}, Deviation: {stat.Deviation}");
        }

        public void MergeToBests(IEntityList entities)
        {
            IEntityList travelers = new EntityList(entities.Count);
            bool anyMerged = false;
            for(int e = 0; e < entities.Count; e++)
            {
                if (entities[e].Fitness.IsLethal()) continue;

                bool merged = FitnessComparer.MergeToBests(Algorithm.Population.Bests, entities[e]);
                if (merged)
                {
                    travelers.Add(entities[e]);
                    var timeString = DateTime.Now.ToString("HH:mm:ss.ffff");
                    Debug.WriteLine(timeString + " " + entities[e].ToString());
                    var stat = Algorithm.Population.FitnessStatistics.StatisticVariables[0];
                    Debug.WriteLine($"{timeString} Mean: {stat.Mean}, Deviation: {stat.Deviation}");
                    anyMerged = true;
                }
            }

            if (anyMerged && NewEntityMergedToBest != null)
            {
                NewEntityMergedToBest(Algorithm.Population.Bests);
            }

            if (LaunchTravelers != null && travelers.Count > 0)
            {
                LaunchTravelers(travelers, TravelerTypes.Best);
            }
        }

        public void TravelersArrived(IEntityList travelers)
        {
            //TODO: Migration replacement
            //foreach (var traveler in travelers)
            //{
            //    FitnessComparer.MergeToBests(Algorithm.Population.Bests, traveler);
            //}

            if (MigrationStrategy.TravelerReceptionDecision(Algorithm.Population))
            {
                MigrationStrategy.InsertMigrants(Algorithm.Population, travelers);
            }
        }
    }
}
