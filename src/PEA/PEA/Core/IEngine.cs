namespace Pea.Core
{
    public interface IEngine
    {
        PeaSettings Settings { get; }
        ParameterSet Parameters { get; }

        IRandom Random { get; set; }
        IAlgorithm Algorithm { get; set; }
        IProvider<IEntityCreator> EntityCreators { get; }
        IEntityCrossover EntityCrossover { get; }
        IEntityMutation EntityMutation { get; }
        IFitnessComparer FitnessComparer { get; }
        IProvider<IReinsertion> Reinsertions { get; }
        IProvider<ISelection> Selections { get; }
        IStopCriteria StopCriteria { get; }

        void Init();
        StopDecision RunOnce();
    }
}