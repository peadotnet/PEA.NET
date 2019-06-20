using System;
using Pea.Core;

namespace Pea.Island
{
    public class IslandEngine : IEngine
    {
        private IAlgorithm Algorithm { get; set; }

        //public ParameterSet ParameterSet { get; set; }

        public IProvider<IEntityCreator> EntityCreators { get; set; }
        public IProvider<ISelection> Selections { get; set; }
        public IPhenotypeDecoder PhenotypeDecoder { get; set; }
        public IFitnessCalculator FitnessCalculator { get; set; }
        public IFitnessComparer FitnessComparer { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public IProvider<IReinsertion> Reinsertions { get; set; }
        public IStopCriteria StopCriteria { get; set; }

        public IslandEngine()
        {

        }

        public void Init()
        {
            Algorithm.InitPopulation();
        }

        public bool RunOnce()
        {
            Algorithm.RunOnce();
            //TODO: StopCriteria
            return true;
        }


    }
}
