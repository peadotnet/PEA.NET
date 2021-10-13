namespace Pea.Core
{
	public delegate IEntityList EvaluationDelegate(IEntityList entityList);

    public interface IAlgorithm
    {
        IEngine Engine { get; }
        IPopulation Population { get; set; }
        void SetEvaluationCallback(EvaluationDelegate evaluationCallback);
        void InitPopulation();
        void RunOnce();
    }
}
