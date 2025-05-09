namespace Pea.Core
{
	public delegate IEntityList EvaluationDelegate(IEntityList entityList);

    public interface IAlgorithm
    {
        IEngine Engine { get; }
        IPopulation Population { get; set; }
        IStopCriteria StopCriteria { get; set; }
        void SetEvaluationCallback(EvaluationDelegate evaluationCallback);
        void InitPopulation(EntityList? entityList = null);
        StopDecision RunOnce();
    }
}
