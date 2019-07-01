using FluentAssertions;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests.PermutationTests
{
    public class PermutationMutationTests
    {
        [Fact]
        public void RelocateRangeMutation_Mutate_ReturnMutated()
        {
            var random = new PredeterminedRandom(new double[] { 5, 3, 2 } );
            var parameterSet = new ParameterSet();
            var chromosome = PermutationTestData.CreateTestChromosome1(); //{ 7, 2, 8, 4, 1, 6, 3, 5, 9, 0 }
            var mutation = new RelocateRangeMutation(random, parameterSet, null);

            var result = mutation.Mutate(chromosome);
            var expected = new int[] {7, 2, 6, 3, 5, 8, 4, 1, 9, 0};

            result.Genes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void InverseRangeMutation_Mutate_ReturnMutated()
        {
            var random = new PredeterminedRandom(new double[] { 5, 4 });
            var parameterSet = new ParameterSet();
            var chromosome = PermutationTestData.CreateTestChromosome1(); //{ 7, 2, 8, 4, 1, 6, 3, 5, 9, 0 }
            var mutation = new InverseRangeMutation(random, parameterSet, null);

            var result = mutation.Mutate(chromosome);
            var expected = new int[] { 7, 2, 6, 3, 1, 4, 8, 5, 9, 0 };

            result.Genes.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SwapTwoRangeMutation_Mutate_ReturnMutated()
        {
            var random = new PredeterminedRandom(new double[] { 5, 3, 2, 2 });
            var parameterSet = new ParameterSet();
            var chromosome = PermutationTestData.CreateTestChromosome1(); //{ 7, 2, 8, 4, 1, 6, 3, 5, 9, 0 }
            var mutation = new SwapTwoRangeMutation(random, parameterSet, null);

            var result = mutation.Mutate(chromosome);
            var expected = new int[] {7, 2, 6, 3, 5, 1, 8, 4, 9, 0};

            result.Genes.Should().BeEquivalentTo(expected);
        }
    }
}
