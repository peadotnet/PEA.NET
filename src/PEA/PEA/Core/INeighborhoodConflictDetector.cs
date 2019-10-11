namespace Pea.Core
{
    public interface INeighborhoodConflictDetector : IConflictDetector
    {
        new bool ConflictDetected(int first, int second);
    }
}
