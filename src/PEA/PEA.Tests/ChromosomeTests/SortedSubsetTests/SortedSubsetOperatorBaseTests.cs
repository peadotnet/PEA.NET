using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class SortedSubsetOperatorBaseTests
    {
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
        public void SubsetOperatorBase_FindNewPosition_ReturnPosition(int sectionIndex, int geneValue, int expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);
            var section = chromosome.Sections[sectionIndex];
            var result = operatorBase.FindNewGenePosition(section, geneValue);
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
        public void SortedSubsetOperatorBase_CountInsertableGenes_Return(int sectionIndex, int insertPosition, int firstGeneIndex, int expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var genesToInsert = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            var random = new FastRandom(DateTime.Now.Millisecond);
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            var result = operatorBase.CountInsertableGenes(chromosome, sectionIndex, insertPosition, genesToInsert, firstGeneIndex);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, null, false)]
        [InlineData(4, null, false)]
        [InlineData(0, 3, false)]
        [InlineData(0, 4, false)]
        [InlineData(1, 4, false)]
        [InlineData(1, 3, true)]
        [InlineData(2, 3, false)]
        [InlineData(2, 4, true)]
        [InlineData(5, 12, false)]
        [InlineData(5, 11, true)]
        public void SortedSubsetOperatorBase_ConflictDetectedWithLeftNeighbor_ShouldReturn(int position, int? geneValue, bool expected)
        {
            var targetSection = new int[] {2, 5, 7, 8, 10};
            var random = new FastRandom(DateTime.Now.Millisecond);
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { new DifferentParityConflictDetector() };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            var result = operatorBase.ConflictDetectedWithLeftNeighbor(targetSection, position, geneValue);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(2, null, false)]
        [InlineData(4, null, false)]
        [InlineData(0, 3, true)]
        [InlineData(2, 3, false)]
        [InlineData(2, 4, true)]
        [InlineData(3, 4, false)]
        [InlineData(3, 3, true)]
        [InlineData(5, 11, false)]
        [InlineData(5, 12, false)]
        public void SortedSubsetOperatorBase_ConflictDetectedWithRightNeighbor_ShouldReturn(int position, int? geneValue, bool expected)
        {
            var targetSection = new int[] { 2, 5, 7, 8, 10 };
            var random = new FastRandom(DateTime.Now.Millisecond);
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { new DifferentParityConflictDetector() };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            var result = operatorBase.ConflictDetectedWithRightNeighbor(targetSection, position, geneValue);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 0, 1, new int[] { 1 })]
        [InlineData(0, 2, 2, new int[] { 5, 7 })]
        [InlineData(1, 0, 2, new int[] { 3, 6 })]
        [InlineData(2, 0, 2, new int[] { 2, 9 })]
        public void SortedSubsetOperatorBase_GetGenes_Return(int sectionIndex, int position, int count, int[] expected)
        {
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            var result = operatorBase.GetGenes(chromosome, sectionIndex, position, count);

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void SortedSubsetOperatorBase_InsertGenesAtPositionWithConflict_ShouldReturns(bool leftConflict, bool rightConflict, bool expectedSuccess)
        {
            var geneValuesToInsert = new int[] { 10, 11, 12, 13 };
            var chromosome = SortedSubsetTestData.CreateChromosome();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { new PredeterminedConflictDetector(leftConflict, rightConflict) };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            var success = operatorBase.InsertGenes(chromosome, 0, 1, geneValuesToInsert, 0, 1);

            success.Should().Be(expectedSuccess);
        }

        [Theory]
        [InlineData(0, 0, 0, 2)]
        [InlineData(0, 2, 1, 2)]
        [InlineData(0, 4, 0, 4)]
        [InlineData(2, 0, 3, 1)]
        [InlineData(2, 1, 1, 3)]
        [InlineData(2, 2, 0, 1)]
        public void SortedSubsetOperatorBase_InsertGenesAtPosition_ShouldInsert(int sectionIndex, int insertPosition, int firstGeneIndex, int count)
        {
            var geneValuesToInsert = new int[] { 10, 11, 12, 13 };

            var chromosome = SortedSubsetTestData.CreateChromosome();
            var clone = chromosome.DeepClone();
            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);
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
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

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
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            operatorBase.DeleteSection(chromosome, sectionIndex);

            chromosome.Sections.GetLength(0).Should().Be(2);
            if (sectionIndex < clone.Sections.GetLength(0) - 1)
            {
                chromosome.Sections[sectionIndex].Should().BeEquivalentTo(clone.Sections[sectionIndex + 1]);
            }
        }

        [Theory]
        [InlineData(0, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        [InlineData(0, 4)]
        public void SortedSubsetOperatorBase_CleanOutSections_DeleteSectionsWith0Length(int sectionIndex1, int sectionIndex2)
        {
            var originalChromosome = SortedSubsetTestData.CreateChromosome();
            var sections = new int[originalChromosome.Sections.Length + 2][];
            var sourceIndex = 0;
            for (int targetIndex = 0; targetIndex < sections.Length; targetIndex++)
            {
                if (targetIndex != sectionIndex1 && targetIndex != sectionIndex2)
                {
                    sections[targetIndex] = originalChromosome.Sections[sourceIndex];
                    sourceIndex++;
                }
                else
                {
                    sections[targetIndex] = new int[0];
                }
            }
            var testChromosome = new SortedSubsetChromosome(sections);

            var random = Substitute.For<IRandom>();
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var operatorBase = new SortedSubsetOperatorBase(random, parameterSet, conflictDetectors);

            operatorBase.CleanOutSections(testChromosome);

            testChromosome.Sections.Should().BeEquivalentTo(originalChromosome.Sections);
        }
    }
}
