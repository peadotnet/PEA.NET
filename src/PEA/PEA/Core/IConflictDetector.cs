namespace Pea.Core
{
	public interface IConflictDetector
	{
		void Init(IEvaluationInitData initData);
	}

	public interface IConflictDetector<T1, T2> : IConflictDetector
	{
		bool ConflictDetected(T1 first, T2 second);
		bool ConflictDetected(IEntity entity, T1 first, T2 second);
	}
}
