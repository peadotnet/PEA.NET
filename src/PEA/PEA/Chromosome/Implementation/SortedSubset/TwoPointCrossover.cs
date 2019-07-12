using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class TwoPointCrossover : SortedSubsetCrossoverBase
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents)
        {
            var children = new List<SortedSubsetChromosome>();

            var sectionsCount = parents[0].Sections.Length > parents[1].Sections.Length
                ? parents[0].Sections.Length
                : parents[1].Sections.Length;

            var totalCount = parents[0].TotalCount > parents[1].TotalCount
                ? parents[0].TotalCount
                : parents[1].TotalCount;

            var child0 = new int[sectionsCount][];
            var child1 = new int[sectionsCount][];
            bool child0Conflicted = false;
            bool child1Conflicted = false;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);
            while (true)
            {
                var crossoverPointLeft = Random.GetInt(0, totalCount);
                var crossoverPointRight = Random.GetIntWithTabu(0, totalCount, crossoverPointLeft);

                if (crossoverPointRight < crossoverPointLeft)
                {
                    var temp = crossoverPointLeft;
                    crossoverPointLeft = crossoverPointRight;
                    crossoverPointRight = temp;
                }

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    var section0 = parents[0].Sections[sectionIndex];
                    var position0left = FindNewGenePosition(section0, crossoverPointLeft);
                    var position0right = FindNewGenePosition(section0, crossoverPointRight);

                    var section1 = parents[1].Sections[sectionIndex];
                    var position1left = FindNewGenePosition(section1, crossoverPointLeft);
                    var position1right = FindNewGenePosition(section1, crossoverPointRight);

                    var child0Section = MergeSections(section0, position0left, position0right, section1, position1left, position1right, ref child0Conflicted);
                    child0[sectionIndex] = child0Section;

                    var child1Section = MergeSections(section1, position1left, position1right, section0, position0left, position0right, ref child1Conflicted);
                    child1[sectionIndex] = child1Section;

                    //TODO: Merge conflict lists

                    if (child0Conflicted && child1Conflicted) break;
                }

                if (!child0Conflicted || !child1Conflicted || retryCount-- < 0) break;

                child0 = new int[sectionsCount][];
                child1 = new int[sectionsCount][];
                child0Conflicted = false;
                child1Conflicted = false;
            }

            if (!child0Conflicted) children.Add(new SortedSubsetChromosome(child0));
            if (!child1Conflicted) children.Add(new SortedSubsetChromosome(child1));

            return children;
        }

        public int[] MergeSections(int[] sectionForOuter, int outerStartPosition, int outerEndPosition, int[] sectionForInner, int innerStartPosition, int innerEndPosition, ref bool childConflicted)
        {
            if (childConflicted) return null;

            int? geneValue0 = (innerStartPosition < sectionForInner.Length)
                ? sectionForInner[innerStartPosition] as int?
                : null;

            int? geneValue1 = (innerEndPosition < sectionForInner.Length)
                ? sectionForInner[innerStartPosition]
                : sectionForInner[sectionForInner.Length-1];

            if (!geneValue0.HasValue) geneValue1 = null;

            if (ConflictDetectedWithLeftNeighbor(sectionForOuter, outerStartPosition, geneValue0))
                childConflicted = true;

            if (ConflictDetectedWithRightNeighbor(sectionForOuter, outerEndPosition, geneValue1))
                childConflicted = true;

            if (childConflicted) return null;

            var innerLength = innerEndPosition - innerStartPosition;
            var rightLength = sectionForOuter.Length - outerEndPosition;

            int[] childSection = new int[outerStartPosition + innerLength + rightLength];

            if (outerStartPosition > 0)
            {
                Array.Copy(sectionForOuter, 0, childSection, 0, outerStartPosition);
            }

            if (innerLength > 0)
            {
                Array.Copy(sectionForInner, innerStartPosition, childSection, outerStartPosition, innerLength);
            }

            if (rightLength > 0)
            {
                Array.Copy(sectionForOuter, outerStartPosition, childSection, outerStartPosition + innerLength, rightLength);
            }

            return childSection;
        }
    }
}
