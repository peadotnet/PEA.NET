using Pea.Core;

namespace Pea.Tests.ChromosomeTests
{
    public class PredeterminedConflictDetector : INeighborhoodConflictDetector
    {
        private int _index = 0;
        private readonly bool[] _results;

        public PredeterminedConflictDetector(params bool[] results)
        {
            _results = results;
        }

        public void Init(IEvaluationInitData initData)
        {
        }

        public bool ConflictDetected(int first, int second)
        {
            bool result = _results[_index];
            _index++;
            if (_index >= _results.Length) _index = 0;
            return result;
        }

		public bool ConflictDetected(IEntity entity, int first, int second)
		{
            return ConflictDetected(first, second);
		}
	}
}
