using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class OnePointCrossover : SortedSubsetCrossoverBase
    {
        public OnePointCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var children = new List<IChromosome>();

            var parent1 = parents[0] as SortedSubsetChromosome;
            var parent2 = parents[1] as SortedSubsetChromosome;

            var sectionsCount = parent1.Sections.Length > parent2.Sections.Length
                ? parent1.Sections.Length 
                : parent2.Sections.Length;

            var totalCount = parent1.TotalCount > parent2.TotalCount 
                ? parent1.TotalCount 
                : parent2.TotalCount;

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
                    bool section0Exists = (parent1.Sections.Length > sectionIndex);
                    var section0 = section0Exists ? parent1.Sections[sectionIndex] : new int[0];
                    var position0 = section0Exists ? FindNewGenePosition(section0, crossoverPosition) : 0;

                    bool section1Exists = (parent2.Sections.Length > sectionIndex);
                    var section1 = section1Exists ? parent2.Sections[sectionIndex] : new int[0];
                    var position1 = section1Exists ? FindNewGenePosition(section1, crossoverPosition) : 0;

                    var child0Section = MergeSections(section0, position0, section1, position1, ref child0Conflicted);
                    child0[sectionIndex] = child0Section;

                    var child1Section = MergeSections(section1, position1, section0, position0, ref child1Conflicted);
                    child1[sectionIndex] = child1Section;

                    //TODO: Merge conflict lists

                    if (child0Conflicted && child1Conflicted) break;
                }

                if (!child0Conflicted || !child1Conflicted)
                {
                    ////TODO: Delete this
                    //var conflictedPositions0 =
                    //    child0Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child0);
                    //var conflictedPositions1 =
                    //    child1Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child1);

                    //if (conflictedPositions0.Count > 0 || conflictedPositions1.Count > 0)
                    //{
                    //    bool error = true;  //For breakpoints
                    //    throw new ApplicationException("Conflict between neighboring values! (Crossover: OnePointCrossover)");
                    //}

                    break;
                }

                if (retryCount-- < 0) break;

                child0 = new int[sectionsCount][];
                child1 = new int[sectionsCount][];
                child0Conflicted = false;
                child1Conflicted = false;
            }

            if (!child0Conflicted)
            {
                var child0Chromosome = new SortedSubsetChromosome(child0);
                CleanOutSections(child0Chromosome);
                children.Add(child0Chromosome);
            }

            if (!child1Conflicted)
            {
                var child1Chromosome = new SortedSubsetChromosome(child1);
                CleanOutSections(child1Chromosome);
                children.Add(child1Chromosome);
            }

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
