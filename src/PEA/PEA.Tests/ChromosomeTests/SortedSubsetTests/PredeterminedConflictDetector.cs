using System;
using Pea.Chromosome.Implementation.SortedSubset;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class PredeterminedConflictDetector : IConflictDetector
    {
        private int index = 0;
        private bool[] _results;

        public PredeterminedConflictDetector(params bool[] results)
        {
            _results = results;
        }

        public bool ConflictDetected(int first, int second)
        {
            bool result = _results[index];
            index++;
            if (index >= _results.Length) index = 0;
            return result;
        }
    }
}
