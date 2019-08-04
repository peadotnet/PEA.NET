namespace Pea.Core
{
    public class AllRightConflictDetector : IConflictDetector
    {
        private AllRightConflictDetector()
        {
        }

        public void Init(IEvaluationInitData initData)
        {
        }

        public bool ConflictDetected(int first, int second)
        {
            return false;
        }

        public static AllRightConflictDetector Instance = new AllRightConflictDetector();
    }
}
