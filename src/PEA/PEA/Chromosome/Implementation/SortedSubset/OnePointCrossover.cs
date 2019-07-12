using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class OnePointCrossover : SortedSubsetCrossoverBase
    {
        public OnePointCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
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
                var crossoverPosition = Random.GetInt(0, totalCount);

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    var section0 = parents[0].Sections[sectionIndex];
                    var position0 = FindNewGenePosition(section0, crossoverPosition);

                    var section1 = parents[1].Sections[sectionIndex];
                    var position1 = FindNewGenePosition(section1, crossoverPosition);

                    var child0Section = MergeSections(section0, position0, section1, position1, ref child0Conflicted);
                    child0[sectionIndex] = child0Section;

                    var child1Section = MergeSections(section1, position1, section0, position0, ref child1Conflicted);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionForLeft"></param>
        /// <param name="leftEndPosition"></param>
        /// <param name="sectionForRight"></param>
        /// <param name="rightStartPosition"></param>
        /// <param name="childConflicted"></param>
        /// <returns></returns>
        public int[] MergeSections(int[] sectionForLeft, int leftEndPosition, int[] sectionForRight, int rightStartPosition, ref bool childConflicted)
        {
            if (childConflicted) return null;

            int? geneValue = (rightStartPosition < sectionForRight.Length)
                ? sectionForRight[rightStartPosition] as int?
                : null;

            if (ConflictDetectedWithLeftNeighbor(sectionForLeft, leftEndPosition, geneValue))
                childConflicted = true;

            if (childConflicted) return null;

            var rightLength = sectionForRight.Length - rightStartPosition;

            int[] childSection =  new int[leftEndPosition + rightLength];

            if (leftEndPosition > 0)
            {
                Array.Copy(sectionForLeft, 0, childSection, 0, leftEndPosition);
            }

            if (rightLength > 0)
            {
                Array.Copy(sectionForRight, rightStartPosition, childSection, leftEndPosition, rightLength);
            }

            return childSection;
        }
    }
}
