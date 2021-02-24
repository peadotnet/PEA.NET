using Pea.Core;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class DifferentParityConflictDetector : INeighborhoodConflictDetector
    {
        public void Init(IEvaluationInitData initData)
        {
        }

        public bool ConflictDetected(int first, int second)
        {
            var parity = (first + second) % 2;
            return (parity == 1);
        }

		public bool ConflictDetected(IEntity entity, int first, int second)
		{
            return ConflictDetected(first, second);
        }
	}
}
