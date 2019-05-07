using System;
using System.Linq;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetOperatorBase
    {
        public IConflictDetector ConflictDetector { get; set; }

        protected readonly IRandom Random;
        protected readonly IParameterSet ParameterSet;

        public SortedSubsetOperatorBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetector = conflictDetector ?? AllRightConflictDetector.Instance;
        }

        public GeneRegion GetSourceSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            return ConflictShouldBeEliminated(chromosome)
                ? GetConflictedSectionAndPosition(chromosome)
                : GetRandomSectionAndPosition(chromosome);
        }

        public GeneRegion GetRandomSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            var sourceSectionIndex = Random.GetInt(0, chromosome.Sections.Length);
            var sourcePosition = Random.GetInt(0, chromosome.Sections[sourceSectionIndex].Length);
            var length = Random.GetInt(1, chromosome.Sections[sourceSectionIndex].Length - sourcePosition);

            var source = new GeneRegion(sourceSectionIndex, sourcePosition, length);
            return source;
        }

        public GeneRegion GetConflictedSectionAndPosition(SortedSubsetChromosome chromosome)
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

        public bool ConflictDetectedWithLeftNeighbor(int[] targetSection, int targetPositionIndex, int? geneValue)
        {
            if (!geneValue.HasValue) return false;
            if (targetPositionIndex == 0) return false;

            var leftNeighborGeneValue = targetSection[targetPositionIndex - 1];
            return ConflictDetector.ConflictDetected(leftNeighborGeneValue, geneValue.Value);
        }

        public bool ConflictDetectedWithRightNeighbor(int[] targetSection, int targetPositionIndex, int? geneValue)
        {
            if (!geneValue.HasValue) return false;
            if (targetPositionIndex == targetSection.Length) return false;

            var rightNeighborGeneValue = targetSection[targetPositionIndex];
            return ConflictDetector.ConflictDetected(geneValue.Value, rightNeighborGeneValue);
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

        public bool ReplaceOneGeneToRandomSection(SortedSubsetChromosome chromosome, GenePosition source)
        {
            var geneValue = chromosome.Sections[source.Section][source.Position];
            var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
            var targetSection = chromosome.Sections[targetSectionIndex];

            var targetPos = FindNewGenePosition(targetSection, geneValue);
            var success = InsertGenes(chromosome, targetSectionIndex, targetPos, chromosome.Sections[source.Section], source.Position, 1);

            if (success) DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);

            return success;
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

            //TODO: collision detection
            return true;
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
            int[][] section = chromosome.Sections;
            int[][] temp = new int[section.Length - 1][];

            if (sectionIndex > 0)
            {
                Array.Copy(section, 0, temp, 0, sectionIndex);
            }

            if (sectionIndex < section.Length)
            {
                Array.Copy(section, sectionIndex + 1, temp, sectionIndex, section.Length - sectionIndex - 1);
            }

            chromosome.Sections = temp;
        }

        public bool ConflictShouldBeEliminated(SortedSubsetChromosome chromosome)
        {
            if (!chromosome.ConflictList.Any()) return false;

            var reducingConflictPossibility = ParameterSet.GetValue(ParameterNames.ConflictReducingPossibility);
            var rnd = Random.GetDouble(0, 1);
            return (rnd < reducingConflictPossibility);
        }
    }
}
