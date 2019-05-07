using System;
using FluentAssertions;
using Pea.Chromosome.Implementation.SortedSubset;
using Xunit;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
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
    }
}
