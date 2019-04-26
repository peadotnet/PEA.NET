namespace Pea.Chromosome.Implementation.SortedSubset
{
    public interface ICollisionDetector
    {
        bool CollisionDetected(int first, int second);
    }
}
