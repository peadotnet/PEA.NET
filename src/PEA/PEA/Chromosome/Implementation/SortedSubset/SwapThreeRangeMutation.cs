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
            if (chromosome.Sections.Length < 2) return null;

            bool childConflicted = false;
            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var source = GetSourceSectionAndPosition(chromosome);
                var firstSection = chromosome.Sections[source.Section];
                var firstPosition1 = source.Position;
                var firstLength = Random.GetInt(0, firstSection.Length - source.Position);
                var firstPosition2 = firstPosition1 + firstLength;

                var firstGeneValue1 = firstSection[firstPosition1];
                var firstGeneValue2 = firstSection[firstPosition2];

                var secondSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
                var secondSection = chromosome.Sections[secondSectionIndex];
                var secondPosition1 = FindNewGenePosition(secondSection, firstGeneValue1);
                var secondPosition2 = FindNewGenePosition(secondSection, firstGeneValue2);

                var secondGeneValue1 = secondSection[secondPosition1];
                var secondGeneValue2 = (secondPosition1 < secondPosition2) ? secondSection[secondPosition2-1] : secondGeneValue1;

                var thirdSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section, secondSectionIndex);
                var thirdSection = chromosome.Sections[thirdSectionIndex];
                var thirdPosition1 = FindNewGenePosition(thirdSection, secondGeneValue1);
                var thirdPosition2 = FindNewGenePosition(thirdSection, secondGeneValue2);

                var temp1 = MergeSections(firstSection, firstPosition1, firstPosition2, secondSection, secondPosition1, secondPosition2, ref childConflicted);
                var temp2 = MergeSections(secondSection, secondPosition1, secondPosition2, thirdSection, thirdPosition1, thirdPosition2, ref childConflicted);
                var temp3 = MergeSections(thirdSection, thirdPosition1, thirdPosition2, firstSection, firstPosition1, firstPosition2, ref childConflicted);


                if (!childConflicted)
                {
                    chromosome.Sections[source.Section] = temp1;
                    chromosome.Sections[secondSectionIndex] = temp2;
                    chromosome.Sections[thirdSectionIndex] = temp3;
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
