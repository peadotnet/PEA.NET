using Pea.Core;

namespace Pea.Chromosome
{
	public interface IConflictDetector
	{
		void Init(IEvaluationInitData initData);
	}

	public interface IConflictDetector<in T1, in T2> : IConflictDetector
	{
		bool ConflictDetected(T1 first, T2 second);
	}
}
