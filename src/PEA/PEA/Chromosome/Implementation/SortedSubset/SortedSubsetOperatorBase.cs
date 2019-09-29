using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetOperatorBase
    {
        public IList<IConflictDetector> ConflictDetectors;

        protected readonly IRandom Random;
        protected readonly IParameterSet ParameterSet;

        public SortedSubsetOperatorBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetectors = conflictDetectors;
        }

        /// <summary>
        /// Returns a new position from the conflict-list or randomly
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public GenePosition GetSourceSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            return ConflictShouldBeEliminated(chromosome)
                ? GetConflictedSectionAndPosition(chromosome)
                : GetRandomSectionAndPosition(chromosome);
        }

        /// <summary>
        /// Returns a new random position
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public GenePosition GetRandomSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            var sectionLength = 0;
            var sourceSectionIndex = 0;
            while (sectionLength == 0)
            {
                sourceSectionIndex = Random.GetInt(0, chromosome.Sections.Length);
                sectionLength = chromosome.Sections[sourceSectionIndex].Length;
            }

            var sourcePosition = Random.GetInt(0, chromosome.Sections[sourceSectionIndex].Length);

            var source = new GenePosition(sourceSectionIndex, sourcePosition);
            return source;
        }

        /// <summary>
        /// Returns a new position randomly choosen from the conflict-list
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public GenePosition GetConflictedSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            var conflicted = Random.GetInt(0, chromosome.ConflictList.Count);
            var source = chromosome.ConflictList[conflicted];
            return source;
        }

        /// <summary>
        /// Returns the position inside the given section where the gene value can be inserted to.
        /// </summary>
        /// <param name="section">The gene section which the gene is intended to insert into</param>
        /// <param name="geneValue">The value of the gene to be inserted</param>
        /// <returns>The position inside the section</returns>
        public int FindNewGenePosition(int[] section, int geneValue)
        {
            if (section.Length == 0) return 0;

            int first = 0;
            int last = section.Length - 1;

            if (geneValue < section[first])
            {
                return first;
            }

            if (geneValue > section[last])
            {
                return last + 1;
            }

            //QuickFind
            while (last > first + 1)
            {
                int middle = (first + last) / 2;

                if (geneValue > section[middle])
                {
                    first = middle;
                }
                else
                {
                    last = middle;
                }
            }

            if (last > 0 && section[last - 1] == geneValue)
                last--;

            return last;
        }

        /// <summary>
        /// Indicates whether the given value gets into conflict with the value
        /// on the left of the target position
        /// </summary>
        /// <param name="targetSection"></param>
        /// <param name="targetPositionIndex"></param>
        /// <param name="geneValue"></param>
        /// <returns></returns>
        public bool ConflictDetectedWithLeftNeighbor(int[] targetSection, int targetPositionIndex, int? geneValue)
        {
            if (!geneValue.HasValue) return false;
            if (targetPositionIndex == 0) return false;

            var leftNeighborGeneValue = targetSection[targetPositionIndex - 1];
            return ConflictDetectors[0].ConflictDetected(leftNeighborGeneValue, geneValue.Value); //TODO: multiple detectors!
        }

        /// <summary>
        /// Indicates whether the given value gets into conflict with the value
        /// on the right of the target position
        /// </summary>
        /// <param name="targetSection"></param>
        /// <param name="targetPositionIndex"></param>
        /// <param name="geneValue"></param>
        /// <returns></returns>
        public bool ConflictDetectedWithRightNeighbor(int[] targetSection, int targetPositionIndex, int? geneValue)
        {
            if (!geneValue.HasValue) return false;
            if (targetPositionIndex == targetSection.Length) return false;

            var rightNeighborGeneValue = targetSection[targetPositionIndex];
            return ConflictDetectors[0].ConflictDetected(geneValue.Value, rightNeighborGeneValue);  //TODO: multiple detectors!
        }

        /// <summary>
        /// Returns the count of genes can be inserted to a given section and position inside it.
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="insertPosition">The position inside the section</param>
        /// <param name="genesToInsert">The gene array to insert</param>
        /// <param name="firstGeneIndex">The first gene's index inside the gene array</param>
        /// <returns>The possible count of the insertable genes</returns>
        public int CountInsertableGenes(SortedSubsetChromosome chromosome, int sectionIndex,
            int insertPosition, int[] genesToInsert, int firstGeneIndex)
        {
            var count = 0;
            while ((firstGeneIndex + count < genesToInsert.Length)
                   && ((insertPosition > chromosome.Sections[sectionIndex].Length - 1)
                       || (genesToInsert[firstGeneIndex + count] < chromosome.Sections[sectionIndex][insertPosition])
                   )
            )
            {
                count++;
            }

            return count;
        }

        public int GetGeneValue(SortedSubsetChromosome chromosome, GenePosition genePosition)
        {
            return chromosome.Sections[genePosition.Section][genePosition.Position];
        }

        /// <summary>
        /// Returns the gene values from a chromosome section
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="position">The position of the first gene to get</param>
        /// <param name="count">The number of genes to get</param>
        /// <returns>The array of gene values</returns>
        public int[] GetGenes(SortedSubsetChromosome chromosome, int sectionIndex, int position, int count)
        {
            int[] result = new int[count];

            Array.Copy(chromosome.Sections[sectionIndex], position, result, 0, count);
            return result;
        }

        /// <summary>
        /// Insert one gene into a chromosome section and position inside it
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="insertPosition">The position inside the section</param>
        /// <param name="genesToInsert">The array of genes to insert</param>
        public bool InsertGenes(SortedSubsetChromosome chromosome, int sectionIndex, int insertPosition, int[] genesToInsert, int firstGeneIndex, int count)
        {
            if (ConflictDetectedWithLeftNeighbor(chromosome.Sections[sectionIndex], insertPosition,
                genesToInsert[firstGeneIndex])) return false;

            if (ConflictDetectedWithRightNeighbor(chromosome.Sections[sectionIndex], insertPosition,
                genesToInsert[firstGeneIndex + count - 1])) return false;

            int[] section = chromosome.Sections[sectionIndex];
            int[] temp = new int[section.Length + count];

            if (insertPosition > 0)
            {
                Array.Copy(section, 0, temp, 0, insertPosition);
            }

            if (count > 0)
            {
                Array.Copy(genesToInsert, firstGeneIndex, temp, insertPosition, count);
            }

            if (insertPosition < section.Length)
            {
                Array.Copy(section, insertPosition, temp, insertPosition + count, section.Length - insertPosition);
            }

            chromosome.Sections[sectionIndex] = temp;

            return true;
        }

        public int[] MergeSections(int[] section1, GeneRange range1, int[] section2, GeneRange range2, ref bool childConflicted)
        {
            if (childConflicted) return null;
            int? geneValue0 = null;
            int? geneValue1 = null;

            if (range2.FirstPosition < range2.LastPosition)
            {
                geneValue0 = section2[range2.FirstPosition];
                geneValue1 = section2[range2.LastPosition - 1];
            }

            if (ConflictDetectedWithLeftNeighbor(section1, range1.FirstPosition, geneValue0))
                childConflicted = true;

            if (ConflictDetectedWithRightNeighbor(section1, range1.LastPosition, geneValue1))
                childConflicted = true;

            if (childConflicted) return null;

            var innerLength = range2.LastPosition - range2.FirstPosition;
            var rightLength = section1.Length - range1.LastPosition;

            int[] childSection = new int[range1.FirstPosition + innerLength + rightLength];

            if (range1.FirstPosition > 0)
            {
                Array.Copy(section1, 0, childSection, 0, range1.FirstPosition);
            }

            if (innerLength > 0)
            {
                Array.Copy(section2, range2.FirstPosition, childSection, range1.FirstPosition, innerLength);
            }

            if (rightLength > 0)
            {
                Array.Copy(section1, range1.LastPosition, childSection, range1.FirstPosition + innerLength, rightLength);
            }

            return childSection;
        }

        /// <summary>
        /// Delete given number of genes from a chromosome section and position inside it
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="position">The position inside the section</param>
        /// <param name="count">The number of genes to be deleted</param>
        public void DeleteGenesFromSection(SortedSubsetChromosome chromosome, int sectionIndex, int position, int count)
        {
            int[] section = chromosome.Sections[sectionIndex];
            int[] temp = new int[section.Length - count];

            if (position > 0)
            {
                Array.Copy(section, 0, temp, 0, position);
            }

            if (position < section.Length - count + 1)
            {
                Array.Copy(section, position + count, temp, position, section.Length - position - count);
            }

            chromosome.Sections[sectionIndex] = temp;
        }

        /// <summary>
        /// Delete one section completely from the chromosome
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section to be deleted</param>
        public void DeleteSection(SortedSubsetChromosome chromosome, int sectionIndex)
        {
            //TODO: simlpy move the sections?
            int[][] sections = chromosome.Sections;
            int[][] temp = new int[sections.Length - 1][];

            if (sectionIndex > 0)
            {
                Array.Copy(sections, 0, temp, 0, sectionIndex);
            }

            if (sectionIndex < sections.Length)
            {
                Array.Copy(sections, sectionIndex + 1, temp, sectionIndex, sections.Length - sectionIndex - 1);
            }

            chromosome.Sections = temp;
        }

        /// <summary>
        /// Deletes all the sections with length of 0
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <returns>True if at least one chromosome is eliminated</returns>
        public bool CleanOutSections(SortedSubsetChromosome chromosome)
        {
            //TODO: simlpy move the sections?
            int[][] sections = chromosome.Sections;

            var sectionsToDeleteCount = 0;
            foreach (var section in sections)
            {
                if (section.Length == 0) sectionsToDeleteCount++;
            }

            if (sectionsToDeleteCount == 0) return false;

            int[][] temp = new int[sections.Length - sectionsToDeleteCount][];

            int sourceStart = 0;
            int targetStart = 0;
            for (int s = 0; s < sections.Length; s++)
            {
                if (sections[s].Length == 0)
                {
                    if (sourceStart < s)
                    {
                        Array.Copy(sections, sourceStart, temp, targetStart, s - sourceStart);
                        targetStart += s - sourceStart;
                    }
                    sourceStart = s + 1;
                }
            }
            if (sourceStart < sections.Length)
            {
                Array.Copy(sections, sourceStart, temp, targetStart, sections.Length - sourceStart);
            }

            chromosome.Sections = temp;

            return true;
        }

        /// <summary>
        /// Make a (mostly random) decision on the using of the conflict list
        /// </summary>
        /// <param name="chromosome"></param>
        /// <returns></returns>
        public bool ConflictShouldBeEliminated(SortedSubsetChromosome chromosome)
        {
            if (chromosome.ConflictList.Count == 0) return false;

            var reducingConflictPossibility = ParameterSet.GetValue(ParameterNames.ConflictReducingProbability);
            var rnd = Random.GetDouble(0, 1);
            return (rnd < reducingConflictPossibility);
        }
    }
}
