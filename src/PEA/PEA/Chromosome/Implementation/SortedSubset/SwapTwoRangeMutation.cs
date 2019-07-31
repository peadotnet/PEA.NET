using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SwapTwoRangeMutation : SortedSubsetMutationBase
    {
        public SwapTwoRangeMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Sections.Length < 2) return null;

            bool childConflicted = false;
            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var source = GetSourceSectionAndPosition(chromosome);
                var sourceSection = chromosome.Sections[source.Section];
                var sourcePosition1 = source.Position;
                var sourceLength = Random.GetInt(0, sourceSection.Length - source.Position);
                var sourcePosition2 = sourcePosition1 + sourceLength;

                var sourceGeneValue1 = sourceSection[sourcePosition1];
                var sourceGeneValue2 = sourceSection[sourcePosition2];

                var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
                var targetSection = chromosome.Sections[targetSectionIndex];

                var targetPosition1 = FindNewGenePosition(targetSection, sourceGeneValue1);
                var targetPosition2 = FindNewGenePosition(targetSection, sourceGeneValue2);

                var temp1 = MergeSections(sourceSection, sourcePosition1, sourcePosition2, targetSection, targetPosition1, targetPosition2, ref childConflicted);
                var temp2 = MergeSections(targetSection, targetPosition1, targetPosition2, sourceSection, sourcePosition1, sourcePosition2, ref childConflicted);

                if (!childConflicted)
                {
                    chromosome.Sections[source.Section] = temp1;
                    chromosome.Sections[targetSectionIndex] = temp2;
                }

                if (!childConflicted || retryCount-- < 0) break;

                childConflicted = false;
            }

            if (childConflicted) return null;

            CleanOutSections(chromosome);
            return chromosome;
        }
    }
}
