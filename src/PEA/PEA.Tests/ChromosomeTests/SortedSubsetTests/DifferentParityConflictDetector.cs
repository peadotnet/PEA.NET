using Pea.Core;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class DifferentParityConflictDetector : IConflictDetector
    {
        public void Init(IEvaluationInitData initData)
        {
        }

        public bool ConflictDetected(int first, int second)
        {
            var parity = (first + second) % 2;
            return (parity == 1);
        }
    }
}
