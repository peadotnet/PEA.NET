namespace Pea.Core
{
    public interface IEngine
    {
        Pea.Configuration.Implementation.PeaSettings Settings { get; }
        ParameterSet Parameters { get; }

        IRandom Random { get; set; }
        IAlgorithm Algorithm { get; set; }
        IEntityCreator EntityCreator { get; }
        IEntityCrossover EntityCrossover { get; }
        IEntityMutation EntityMutation { get; }
        IFitnessComparer FitnessComparer { get; }
        IProvider<IReinsertion> Reinsertions { get; }
        IProvider<ISelection> Selections { get; }
        IStopCriteria StopCriteria { get; }
        IEvaluation Evaluation { get; set; }

        void Init(IEvaluationInitData initData);
        StopDecision RunOnce();
    }
}