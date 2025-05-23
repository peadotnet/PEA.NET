﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pea.Core.Events;
using Pea.Migration;
using Pea.Restart;

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
        public EvaluationBase Evaluation { get; set; }
        public IMigrationStrategy MigrationStrategy { get; set; }

        public IRestartStategy RestartStategy { get; set; }// = new UnchangedMeanRestartStrategy();
        public LaunchTravelersDelegate LaunchTravelers { get; set; }
        public int Iteration { get; set; } = 0;

        private bool _initialization = false;

        public event NewEntitiesMergedToBestDelegate NewEntityMergedToBest;

        public IslandEngine()
        {
             
        }

        public void Init(IEvaluationInitData initData)
        {
            _initialization = true;
            initData.Build();
            InitConflictDetectors(initData);
            InitEntityCreators(initData);
            Algorithm.InitPopulation();
            _initialization = false;
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
            Iteration++;

            if (RestartStategy != null && RestartStategy.ShouldRestart(Iteration, Algorithm.Population))
            {
                _initialization = true;
                var remainingEntities = RestartStategy.GetRemainingEntities(Algorithm.Population);
                Algorithm.InitPopulation(remainingEntities);
                _initialization = false;
            }

            return Algorithm.RunOnce();
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
                    //Debug.WriteLine($"{timeString} Mean: {stat.Mean}, Deviation: {stat.Deviation}");
                    anyMerged = true;
                }
            }

            if (!_initialization && anyMerged && NewEntityMergedToBest != null)
            {
                NewEntityMergedToBest(
                    new NewEntitiesMergedToBestEventArgs()
                    {
                        Iteration = Iteration,
                        BestEntities = Algorithm.Population.Bests,
                        FitnessStatistics = Algorithm.Population.FitnessStatistics
                    });
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

        public IEvolutionStateReportData GetCurrentState()
        {
            return new NewEntitiesMergedToBestEventArgs()
            {
                Iteration = Iteration,
                BestEntities = Algorithm.Population.Bests,
                FitnessStatistics = Algorithm.Population.FitnessStatistics
            };
        }
    }
}
