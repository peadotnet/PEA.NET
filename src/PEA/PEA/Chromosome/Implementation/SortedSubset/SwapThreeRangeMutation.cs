using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SwapThreeRangeMutation : SortedSubsetMutationBase
    {
        public SwapThreeRangeMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Sections.Length < 3) return null;

            bool childConflicted = false;
            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var firstRange = GetSourceRange(chromosome);
                var firstSection = chromosome.Sections[firstRange.Section];
                var firstGeneValue1 = firstSection[firstRange.FirstPosition];
                var firstGeneValue2 = firstSection[firstRange.LastPosition - 1];

                var secondSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, firstRange.Section);
                var secondSection = chromosome.Sections[secondSectionIndex];
                var secondPosition1 = FindNewGenePosition(secondSection, firstGeneValue1);
                var secondPosition2 = FindNewGenePosition(secondSection, firstGeneValue2);
                var secondRange = new GeneRange(secondSectionIndex, secondPosition1, secondPosition2);

                if (secondRange.Length == 0) childConflicted = true;

                if (!childConflicted)
                {
                    var secondGeneValue1 = secondSection[secondRange.FirstPosition];
                    var secondGeneValue2 = secondSection[secondRange.LastPosition - 1];

                    var thirdSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, firstRange.Section,
                        secondRange.Section);
                    var thirdSection = chromosome.Sections[thirdSectionIndex];
                    var thirdPosition1 = FindNewGenePosition(thirdSection, secondGeneValue1);
                    var thirdPosition2 = FindNewGenePosition(thirdSection, secondGeneValue2);
                    var thirdRange = new GeneRange(thirdSectionIndex, thirdPosition1, thirdPosition2);

                    var temp1 = MergeSections(firstSection, firstRange, secondSection, secondRange,
                        ref childConflicted);
                    var temp2 = MergeSections(secondSection, secondRange, thirdSection, thirdRange,
                        ref childConflicted);
                    var temp3 = MergeSections(thirdSection, thirdRange, firstSection, firstRange, ref childConflicted);

                    if (!childConflicted)
                    {
                        chromosome.Sections[firstRange.Section] = temp1;
                        chromosome.Sections[secondRange.Section] = temp2;
                        chromosome.Sections[thirdRange.Section] = temp3;
                    }
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
