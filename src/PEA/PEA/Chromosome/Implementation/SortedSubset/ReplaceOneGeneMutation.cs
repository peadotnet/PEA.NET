using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class ReplaceOneGeneMutation : SortedSubsetMutationBase
    {
        public ReplaceOneGeneMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Sections.Length < 2) return null;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var source = GetSourceSectionAndPosition(chromosome);
                bool success = ReplaceOneGeneToRandomSection(chromosome, source, retryCount);

                if (success || retryCount-- < 0) break;
            }

            CleanOutSections(chromosome);
            return chromosome;
        }
    }
}
