using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SwapTwoRangeMutation : SortedSubsetMutationBase
    {
        public SwapTwoRangeMutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
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

                var range1 = GetSourceRange(chromosome);
                var section1 = chromosome.Sections[range1.Section];
                var sourceGeneValue1 = section1[range1.FirstPosition];
                var sourceGeneValue2 = section1[range1.LastPosition - 1];

                var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, range1.Section);
                var section2 = chromosome.Sections[targetSectionIndex];
                var targetPosition1 = FindNewGenePosition(section2, sourceGeneValue1);
                var targetPosition2 = FindNewGenePosition(section2, sourceGeneValue2);
                var range2 = new GeneRange(targetSectionIndex, targetPosition1, targetPosition2);

                var temp1 = MergeSections(section1, range1, section2, range2, ref childConflicted);
                var temp2 = MergeSections(section2, range2, section1, range1, ref childConflicted);

                if (!childConflicted)
                {
                    chromosome.Sections[range1.Section] = temp1;
                    chromosome.Sections[range2.Section] = temp2;
                }

                if (!childConflicted || retryCount-- < 0)
                {
                    //TODO: Remove this (only for debugging purpose)
                    chromosome.GeneratedRandoms.Add(range1.Section);
                    chromosome.GeneratedRandoms.Add(range1.FirstPosition);
                    chromosome.GeneratedRandoms.Add(range1.LastPosition);
                    chromosome.GeneratedRandoms.Add(range2.Section);
                    chromosome.GeneratedRandoms.Add(range2.FirstPosition);
                    chromosome.GeneratedRandoms.Add(range2.LastPosition);

                    break;
                }

                childConflicted = false;
            }

            if (childConflicted) return null;

            CleanOutSections(chromosome);
            return chromosome;
        }
    }
}
