using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class ReplaceOneGeneMutation : SortedSubsetMutationBase
    {
        public ReplaceOneGeneMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();
            if (chromosome.Sections.Length < 2) return null;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var source = GetSourceSectionAndPosition(chromosome);
                bool success = ReplaceOneGeneToRandomSection(chromosome, source);

                if (success || retryCount-- < 0) break;
            }
            return chromosome;
        }
    }
}
