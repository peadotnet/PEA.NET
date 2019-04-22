using System;

namespace Pea.Genotype.Implementation.SortedSubset
{
    public class SortedSubsetOperatorBase
    {
        /// <summary>
        /// Returns the position inside the given chromosome where the gene value can be inserted to.
        /// </summary>
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome</param>
        /// <param name="geneValue">The value of the gene to be insert</param>
        /// <returns>The position inside the chromosome</returns>
        public int FindNewGenePosition(SortedSubsetGenotype genotype, int chromosomeIndex, int geneValue)
        {
            var chromosome = genotype.Chromosomes[chromosomeIndex];

            int first = 0;
            int last = chromosome.Length - 1;

            if (geneValue < chromosome[first])
            {
                return first;
            }

            if (geneValue > chromosome[last])
            {
                return last + 1;
            }

            //QuickFind
            while (last > first + 1)
            {
                int middle = (first + last) / 2;

                if (geneValue > chromosome[middle])
                {
                    first = middle;
                }
                else
                {
                    last = middle;
                }
            }

            if (last > 0 && chromosome[last - 1] == geneValue)
                last--;

            return last;
        }

        /// <summary>
        /// Returns the count of genes can be inserted to a given chromosome and position inside it.
        /// </summary>
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome</param>
        /// <param name="insertPosition">The position inside the chromosome</param>
        /// <param name="genesToInsert">The gene array to insert</param>
        /// <param name="firstGeneIndex">The first gene's index inside the gene array</param>
        /// <returns>The possible count of the insertable genes</returns>
        public int CountInsertableGenes(SortedSubsetGenotype genotype, int chromosomeIndex,
            int insertPosition, int[] genesToInsert, int firstGeneIndex)
        {
            var count = 0;
            while ((firstGeneIndex + count < genesToInsert.Length)
                    && ((insertPosition > genotype.Chromosomes[chromosomeIndex].Length - 1)
                         || (genesToInsert[firstGeneIndex + count] < genotype.Chromosomes[chromosomeIndex][insertPosition])
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
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome</param>
        /// <param name="position">The position of the first gene to get</param>
        /// <param name="count">The number of genes to get</param>
        /// <returns></returns>
        public int[] GetGenes(SortedSubsetGenotype genotype, int chromosomeIndex, int position, int count)
        {
            int[] result = new int[count];

            Array.Copy(genotype.Chromosomes[chromosomeIndex], position, result, 0, count);
            return result;
        }

        /// <summary>
        /// Insert one gene into a chromosome and position inside it
        /// </summary>
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome</param>
        /// <param name="insertPosition">The position inside the chromosome</param>
        /// <param name="genesToInsert"></param>
        public void InsertGenes(SortedSubsetGenotype genotype, int chromosomeIndex, int insertPosition, int[] genesToInsert, int firstGeneIndex, int count)
        {
            int[] chromosome = genotype.Chromosomes[chromosomeIndex];
            int[] temp = new int[chromosome.Length + count];

            if (insertPosition > 0)
            {
                Array.Copy(chromosome, 0, temp, 0, insertPosition);
            }

            if (count > 0)
            {
                Array.Copy(genesToInsert, firstGeneIndex, temp, insertPosition, count);
            }

            if (insertPosition < chromosome.Length)
            {
                Array.Copy(chromosome, insertPosition, temp, insertPosition + count, chromosome.Length - insertPosition);
            }

            genotype.Chromosomes[chromosomeIndex] = temp;
        }

        /// <summary>
        /// Delete given number of genes from a chromosome and position inside it
        /// </summary>
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome</param>
        /// <param name="position">The position inside the chromosome</param>
        /// <param name="count">The number of genes to be deleted</param>
        public void DeleteGenesFromChromosome(SortedSubsetGenotype genotype, int chromosomeIndex, int position, int count)
        {
            int[] chromosome = genotype.Chromosomes[chromosomeIndex];
            int[] temp = new int[chromosome.Length - count];

            if (position > 0)
            {
                Array.Copy(chromosome, 0, temp, 0, position);
            }

            if (position < chromosome.Length - count + 1)
            {
                Array.Copy(chromosome, position + count, temp, position, chromosome.Length - position - count);
            }

            genotype.Chromosomes[chromosomeIndex] = temp;
        }

        /// <summary>
        /// Delete one chromosome completely from the genotype
        /// </summary>
        /// <param name="genotype">The multichromosome genotype which the operator works within</param>
        /// <param name="chromosomeIndex">The index of the chromosome to be deleted</param>
        public void DeleteChromosome(SortedSubsetGenotype genotype, int chromosomeIndex)
        {
            int[][] chromosome = genotype.Chromosomes;
            int[][] temp = new int[chromosome.Length - 1][];

            if (chromosomeIndex > 0)
            {
                Array.Copy(chromosome, 0, temp, 0, chromosomeIndex);
            }

            if (chromosomeIndex < chromosome.Length)
            {
                Array.Copy(chromosome, chromosomeIndex + 1, temp, chromosomeIndex, chromosome.Length - chromosomeIndex - 1);
            }

            genotype.Chromosomes = temp;
        }
    }
}
