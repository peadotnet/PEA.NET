using System;
using FluentAssertions;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests.PermutationTests
{
    public class PermutationCrossoverTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 9)]
        [InlineData(7, 7)]
        [InlineData(8, 8)]
        [InlineData(9, 2)]
        public void PMXCrossover_CreateGeneMap_GetUniqueValue_ReturnUnique(int position, int expected)
        {
            var random = new PredeterminedRandom(new double[0]);
            var parameterSet = new ParameterSet();
            var crossover = new PMXCrossover(random, parameterSet, null);
            var chromosomes = PermutationTestData.CreateTestChromosomes();
            var range = new GeneRange(3, 4);
            var geneMap = crossover.GenerateGeneMap(((PermutationChromosome)chromosomes[0]).Genes, range);

            var result = crossover.GetUniqueGeneValue(((PermutationChromosome)chromosomes[1]).Genes, geneMap, position);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(0, 3, new int[] { 0, 5, 9 })]
        [InlineData(6, 10, new int[] { 2, 7, 8, 3 })]
        public void PMXCrossover_CopyWithDuplicationElimination_CopyData(int begin, int end, int[] expected)
        {
            var random = new PredeterminedRandom(new double[0]);
            var parameterSet = new ParameterSet();
            var crossover = new PMXCrossover(random, parameterSet, null);
            var chromosomes = PermutationTestData.CreateTestChromosomes();
            var range = new GeneRange(3, 3);
            var geneMap = crossover.GenerateGeneMap(((PermutationChromosome)chromosomes[0]).Genes, range);
            var childGenes = new int[((PermutationChromosome)chromosomes[0]).Genes.Length];

            var length = end - begin;
            crossover.CopyWithDuplicationElimination(((PermutationChromosome)chromosomes[1]).Genes, childGenes, geneMap, begin, end);
            var result = new int[length];
            Array.Copy(childGenes, begin, result, 0, length);

            result.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(3, 3, new int[] { 0, 5, 9, 4, 1, 6, 2, 7, 8, 3 })]
        [InlineData(3, 4, new int[] { 0, 5, 9, 4, 1, 6, 3, 7, 8, 2 })]
        public void PMXCrossover_Cross_ReturnChildren(int position, int length, int[] expected)
        {
            var random = new PredeterminedRandom(new double[] { position, length } );
            var parameterSet = new ParameterSet();
            var crossover = new PMXCrossover(random, parameterSet, null);
            var parents = PermutationTestData.CreateTestChromosomes();

            var children = crossover.Cross(parents);

            children.Count.Should().Be(2);
            ((PermutationChromosome)children[0]).Genes.Should().BeEquivalentTo(expected);
        }
    }
}
