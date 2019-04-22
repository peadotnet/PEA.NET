using Xunit;
using System;
using FluentAssertions;
using Pea.Genotype.Implementation.SortedSubset;

namespace PGA.NET.Tests.GenotypeTests
{
    public class SortedSubsetGenotypeTests
    {
        [Fact]
        public void WhenCreateSortedSubsetGenotype_WithNull_ThenShouldThrow()
        {
            Action act = () => new SortedSubsetGenotype(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenCreateSortedSubsetGenotype_WithNullNested_ThenShouldThrow()
        {
            Action act = () => new SortedSubsetGenotype(new int[][]
            {
                new int[] { 1, 2 },
                null
            });
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenSortedSubsetGenotype_WhenDeepClone_ThenShouldBeEquivalent()
        {
            var genotype = new SortedSubsetGenotype(new int[][]
            {
                new int[] { 1, 2 },
                new int[] { 3,4,5}
            });

            var clone = genotype.DeepClone();

            clone.Chromosomes.Should().BeEquivalentTo(genotype.Chromosomes);
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 3, 1)]
        [InlineData(0, 4, 1)]
        [InlineData(0, 6, 3)]
        [InlineData(0, 8, 4)]
        [InlineData(1, 2, 0)]
        [InlineData(1, 7, 2)]
        [InlineData(1, 9, 3)]
        [InlineData(2, 1, 0)]
        [InlineData(2, 3, 1)]
        [InlineData(2, 9, 1)]
        [InlineData(2, 10, 2)]
        public void GivenSortedSubsetOperatorBase_WhenFindNewPosition_ThenReturns(int chromosomeIndex, int geneValue, int expected)
        {
            var genotype = CreateGenotype();

            var operatorBase = new SortedSubsetOperatorBase();
            var result = operatorBase.FindNewGenePosition(genotype, chromosomeIndex, geneValue);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0, 0, 1)]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 1, 2, 2)]
        [InlineData(0, 1, 3, 1)]
        [InlineData(0, 2, 3, 2)]
        [InlineData(0, 2, 5, 0)]
        [InlineData(0, 2, 6, 0)]
        [InlineData(0, 3, 6, 1)]
        [InlineData(0, 4, 8, 4)]
        [InlineData(2, 1, 0, 9)]
        [InlineData(2, 1, 6, 3)]
        [InlineData(2, 2, 10, 2)]
        public void GivenSortedSubsetOperatorBase_WhenCountInsertableGenes_ThenReturns(int chromosomeIndex, int insertPosition, int firstGeneIndex, int expected)
        {
            var genotype = CreateGenotype();
            var genesToInsert = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var operatorBase = new SortedSubsetOperatorBase();
            var result = operatorBase.CountInsertableGenes(genotype, chromosomeIndex, insertPosition, genesToInsert, firstGeneIndex);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0, 1, new int[] { 1 })]
        [InlineData(0, 2, 2, new int[] { 5, 7 })]
        [InlineData(1, 0, 2, new int[] { 3, 6 })]
        [InlineData(2, 0, 2, new int[] { 2, 9 })]
        public void GivenSortedSubsetOperatorBase_WhenGetGenes_ThenReturns(int chromosomeIndex, int position, int count, int[] expected)
        {
            var genotype = CreateGenotype();
            var operatorBase = new SortedSubsetOperatorBase();
            var result = operatorBase.GetGenes(genotype, chromosomeIndex, position, count);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0, 0, 2)]
        [InlineData(0, 2, 1, 2)]
        [InlineData(0, 4, 0, 4)]
        [InlineData(2, 0, 3, 1)]
        [InlineData(2, 1, 1, 3)]
        [InlineData(2, 2, 0, 1)]
        public void GivenSortedSubsetOperatorBase_WhenInsertGeneAtPosition_ThenShouldInsert(int chromosomeIndex, int insertPosition, int firstGeneIndex, int count)
        {
            var geneValuesToInsert = new int[] { 10, 11, 12, 13 };

            var genotype = CreateGenotype();
            var clone = genotype.DeepClone();
            var operatorBase = new SortedSubsetOperatorBase();
            operatorBase.InsertGenes(genotype, chromosomeIndex, insertPosition, geneValuesToInsert, firstGeneIndex, count);

            genotype.Chromosomes[chromosomeIndex].Length.Should().Be(clone.Chromosomes[chromosomeIndex].Length + count);
            genotype.Chromosomes[chromosomeIndex][insertPosition].Should().Be(geneValuesToInsert[firstGeneIndex]);
            genotype.Chromosomes[chromosomeIndex][insertPosition + count - 1].Should().Be(geneValuesToInsert[firstGeneIndex + count - 1]);
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 0, 3)]
        [InlineData(0, 0, 4)]
        [InlineData(1, 2, 1)]
        [InlineData(2, 0, 2)]
        public void GivenSortedSubsetOperatorBase_WhenDeleteGenes_ThenShouldDelete(int chromosomeIndex, int position, int count)
        {
            var genotype = CreateGenotype();
            var clone = genotype.DeepClone();
            var operatorBase = new SortedSubsetOperatorBase();
            operatorBase.DeleteGenesFromChromosome(genotype, chromosomeIndex, position, count);

            genotype.Chromosomes[chromosomeIndex].Length.Should().Be(clone.Chromosomes[chromosomeIndex].Length - count);
            if (position + count < clone.Chromosomes[chromosomeIndex].Length)
            {
                genotype.Chromosomes[chromosomeIndex][position].Should().Be(clone.Chromosomes[chromosomeIndex][position + count]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GivenSortedSubsetOperatorBase_WhenDeleteChromosome_ThenShouldDelete(int chromosomeIndex)
        {
            var genotype = CreateGenotype();
            var clone = genotype.DeepClone();
            var operatorBase = new SortedSubsetOperatorBase();
            operatorBase.DeleteChromosome(genotype, chromosomeIndex);
            genotype.Chromosomes.GetLength(0).Should().Be(2);
            if (chromosomeIndex < clone.Chromosomes.GetLength(0) - 1)
            {
                genotype.Chromosomes[chromosomeIndex].Should().BeEquivalentTo(clone.Chromosomes[chromosomeIndex + 1]);
            }
        }

        private static SortedSubsetGenotype CreateGenotype()
        {
            var chromosome1 = new int[] { 1, 4, 5, 7 };
            var chromosome2 = new int[] { 3, 6, 8 };
            var chromosome3 = new int[] { 2, 9 };

            var chromosomes = new int[][]
            {
                chromosome1, chromosome2, chromosome3
            };

            var genotype = new SortedSubsetGenotype(chromosomes);

            return genotype;
        }
    }
}
