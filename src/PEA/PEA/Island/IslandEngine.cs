using Pea.Core;

namespace Pea.Island
{
    public class IslandEngine : IEngine
    {
        public IProvider<IEntityCreator> EntityCreators { get; set; }
        public IProvider<ISelection> Selectors { get; set; }
        public IPhenotypeDecoder PhenotypeDecoder { get; set; }
        public IFitnessCalculator FitnessCalculator { get; set; }
        public IFitnessComparer FitnessComparer { get; set; }
        public IEntityCrossover EntityCrossover { get; set; }
        public IEntityMutation EntityMutation { get; set; }
        public IProvider<IReinsertion> Reinsertions { get; set; }

        private IAlgorithm Algorithm { get; set; }

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
