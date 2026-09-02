namespace Pea.Core
{
	public delegate IEntityList EvaluationDelegate(IEntityList entityList);

    public interface IAlgorithm
    {
        IPopulation Population { get; }
        IStopCriteria StopCriteria { get; set; }


        IProvider<IEntityCreator> EntityCreators { get; }
        IProvider<ISelection> Selections { get; set; }
        IEntityCrossover EntityCrossover { get; set; }
        IEntityMutation EntityMutation { get; set; }
        IFitnessComparer FitnessComparer { get; set; }
        IProvider<IReplacement> Replacements { get; set; }


        void SetEvaluationCallback(EvaluationDelegate evaluationCallback);
        void InitPopulation(EntityList? entityList = null);
        StopDecision RunOnce();
    }
}
