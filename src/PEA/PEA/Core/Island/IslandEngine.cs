using System.Collections.Generic;

namespace Pea.Core.Island
{
    public class IslandEngine : IEngine
    {
        public IAlgorithm Algorithm { get; set; }

        //public ParameterSet ParameterSet { get; set; }
        public IRandom Random { get; set; }

        public Pea.Configuration.Implementation.PeaSettings Settings { get; set; }
        public ParameterSet Parameters { get; set; }
        public IList<IConflictDetector> ConflictDetectors { get; set; }
        public IEntityCreator EntityCreator { get; set; }
        public IProvider<ISelection> Selections { get; set; }
        public IFitnessComparer FitnessComparer { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public IProvider<IReinsertion> Reinsertions { get; set; }
        public IStopCriteria StopCriteria { get; set; }

        public IslandEngine()
        {

        }

        public void Init(IEvaluationInitData initData)
        {
            initData.Build();
            foreach (var conflictDetector in ConflictDetectors)
            {
                conflictDetector.Init(initData);
            }
            Algorithm.InitPopulation();
        }

        public StopDecision RunOnce()
        {
            Algorithm.RunOnce();
            return StopCriteria.MakeDecision(this, Algorithm.Population);
        }
    }
}
