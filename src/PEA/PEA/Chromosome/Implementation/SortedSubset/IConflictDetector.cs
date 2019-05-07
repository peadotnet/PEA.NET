namespace Pea.Chromosome.Implementation.SortedSubset
{
    public interface IConflictDetector
    {
        bool ConflictDetected(int first, int second);
    }
}
