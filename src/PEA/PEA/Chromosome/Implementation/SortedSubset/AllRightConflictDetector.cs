using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class AllRightConflictDetector : IConflictDetector
    {
        private AllRightConflictDetector()
        {
        }

        public bool ConflictDetected(int first, int second)
        {
            return false;
        }

        public static AllRightConflictDetector Instance = new AllRightConflictDetector();
    }
}
