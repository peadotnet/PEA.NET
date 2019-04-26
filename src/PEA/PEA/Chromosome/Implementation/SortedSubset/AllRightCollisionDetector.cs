namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class AllRightCollisionDetector : ICollisionDetector
    {
        private AllRightCollisionDetector()
        {
        }

        public bool CollisionDetected(int first, int second)
        {
            return false;
        }

        public static AllRightCollisionDetector Instance = new AllRightCollisionDetector();
    }
}
