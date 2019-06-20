namespace Pea.Core
{
    public interface IEngine
    {
        IProvider<IEntityCreator> EntityCreators { get; set; }
        IEntityCrossover EntityCrossover { get; set; }
        IEntityMutation EntityMutation { get; set; }
        IFitnessCalculator FitnessCalculator { get; set; }
        IFitnessComparer FitnessComparer { get; set; }
        IPhenotypeDecoder PhenotypeDecoder { get; set; }
        IProvider<IReinsertion> Reinsertions { get; set; }
        IProvider<ISelection> Selections { get; set; }
        IStopCriteria StopCriteria { get; set; }

        bool RunOnce();
    }
}