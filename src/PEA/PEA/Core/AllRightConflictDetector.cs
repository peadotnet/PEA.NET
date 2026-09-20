using Pea.Core.Entity;

namespace Pea.Core
{
    public class AllRightConflictDetector : INeighborhoodConflictDetector
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

		public bool ConflictDetected(EntityBase entity, int first, int second)
		{
            return false;
		}

		public static AllRightConflictDetector Instance = new AllRightConflictDetector();
    }
}
