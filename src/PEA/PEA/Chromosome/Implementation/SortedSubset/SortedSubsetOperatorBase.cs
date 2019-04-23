using System;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetOperatorBase
    {
        /// <summary>
        /// Returns the position inside the given section where the gene value can be inserted to.
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="geneValue">The value of the gene to be insert</param>
        /// <returns>The position inside the section</returns>
        public int FindNewGenePosition(SortedSubsetChromosome chromosome, int sectionIndex, int geneValue)
        {
            var section = chromosome.Sections[sectionIndex];

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

        /// <summary>
        /// Insert one gene into a chromosome section and position inside it
        /// </summary>
        /// <param name="chromosome">The multi-section chromosome which the operator works within</param>
        /// <param name="sectionIndex">The index of the section</param>
        /// <param name="insertPosition">The position inside the section</param>
        /// <param name="genesToInsert">The array of genes to insert</param>
        public void InsertGenes(SortedSubsetChromosome chromosome, int sectionIndex, int insertPosition, int[] genesToInsert, int firstGeneIndex, int count)
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
    }
}
