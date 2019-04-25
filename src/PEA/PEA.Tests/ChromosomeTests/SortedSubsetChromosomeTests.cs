using System;
using FluentAssertions;
using NSubstitute;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests
{
    public class SortedSubsetChromosomeTests
    {
        [Fact]
        public void WhenCreateSortedSubsetChromosome_WithNull_ThenShouldThrow()
        {
            Action act = () => new SortedSubsetChromosome(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void WhenCreateSortedSubsetChromosome_WithNullNested_ThenShouldThrow()
        {
            Action act = () => new SortedSubsetChromosome(new int[][]
            {
                new int[] { 1, 2 },
                null
            });
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenSortedSubsetChromosome_WhenDeepClone_ThenShouldBeEquivalent()
        {
            var chromosome = new SortedSubsetChromosome(new int[][]
            {
                new int[] { 1, 2 },
                new int[] { 3,4,5}
            });

            var clone = chromosome.DeepClone();

            clone.Sections.Should().BeEquivalentTo(chromosome.Sections);
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
        public void GivenSortedSubsetOperatorBase_WhenFindNewPosition_ThenReturns(int sectionIndex, int geneValue, int expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);
            var result = operatorBase.FindNewGenePosition(chromosome, sectionIndex, geneValue);
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
        public void GivenSortedSubsetOperatorBase_WhenCountInsertableGenes_ThenReturns(int sectionIndex, int insertPosition, int firstGeneIndex, int expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var genesToInsert = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);
            var result = operatorBase.CountInsertableGenes(chromosome, sectionIndex, insertPosition, genesToInsert, firstGeneIndex);
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0, 1, new int[] { 1 })]
        [InlineData(0, 2, 2, new int[] { 5, 7 })]
        [InlineData(1, 0, 2, new int[] { 3, 6 })]
        [InlineData(2, 0, 2, new int[] { 2, 9 })]
        public void GivenSortedSubsetOperatorBase_WhenGetGenes_ThenReturns(int sectionIndex, int position, int count, int[] expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);
            var result = operatorBase.GetGenes(chromosome, sectionIndex, position, count);
            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(0, 0, 0, 2)]
        [InlineData(0, 2, 1, 2)]
        [InlineData(0, 4, 0, 4)]
        [InlineData(2, 0, 3, 1)]
        [InlineData(2, 1, 1, 3)]
        [InlineData(2, 2, 0, 1)]
        public void GivenSortedSubsetOperatorBase_WhenInsertGeneAtPosition_ThenShouldInsert(int sectionIndex, int insertPosition, int firstGeneIndex, int count)
        {
            var geneValuesToInsert = new int[] { 10, 11, 12, 13 };

            var chromosome = SortedSubsetTestData.CreateChromosome();
            var clone = chromosome.DeepClone();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);
            operatorBase.InsertGenes(chromosome, sectionIndex, insertPosition, geneValuesToInsert, firstGeneIndex, count);

            chromosome.Sections[sectionIndex].Length.Should().Be(clone.Sections[sectionIndex].Length + count);
            chromosome.Sections[sectionIndex][insertPosition].Should().Be(geneValuesToInsert[firstGeneIndex]);
            chromosome.Sections[sectionIndex][insertPosition + count - 1].Should().Be(geneValuesToInsert[firstGeneIndex + count - 1]);
        }

        [Theory]
        [InlineData(0, 0, 1)]
        [InlineData(0, 0, 3)]
        [InlineData(0, 0, 4)]
        [InlineData(1, 2, 1)]
        [InlineData(2, 0, 2)]
        public void GivenSortedSubsetOperatorBase_WhenDeleteGenes_ThenShouldDelete(int sectionIndex, int position, int count)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var clone = chromosome.DeepClone();

            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);

            operatorBase.DeleteGenesFromSection(chromosome, sectionIndex, position, count);

            chromosome.Sections[sectionIndex].Length.Should().Be(clone.Sections[sectionIndex].Length - count);
            if (position + count < clone.Sections[sectionIndex].Length)
            {
                chromosome.Sections[sectionIndex][position].Should().Be(clone.Sections[sectionIndex][position + count]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GivenSortedSubsetOperatorBase_WhenDeleteSection_ThenShouldDelete(int sectionIndex)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var clone = chromosome.DeepClone();

            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet);

            operatorBase.DeleteSection(chromosome, sectionIndex);
            chromosome.Sections.GetLength(0).Should().Be(2);
            if (sectionIndex < clone.Sections.GetLength(0) - 1)
            {
                chromosome.Sections[sectionIndex].Should().BeEquivalentTo(clone.Sections[sectionIndex + 1]);
            }
        }


    }
}
