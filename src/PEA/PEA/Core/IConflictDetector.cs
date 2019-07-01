namespace Pea.Core
{
    public interface IConflictDetector
    {
        bool ConflictDetected(int first, int second);
    }
}
