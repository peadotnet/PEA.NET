namespace Pea.Core
{
    public interface IConflictDetector
    {
        void Init(IEvaluationInitData initData);
        bool ConflictDetected(int first, int second);
    }
}
