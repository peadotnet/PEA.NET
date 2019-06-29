namespace Pea.Chromosome.Implementation.Permutation
{
    public interface IConflictDetector
    {
        bool ConflictDetected(int first, int second);
    }
}
