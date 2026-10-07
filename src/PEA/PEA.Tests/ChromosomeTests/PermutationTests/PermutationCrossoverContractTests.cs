using System.Collections.Generic;
using FluentAssertions;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests.PermutationTests
{
    public class PermutationCrossoverContractTests
    {
        public static IEnumerable<object[]> Crossovers()
        {
            yield return new object[] { nameof(Order1Crossover) };
            yield return new object[] { nameof(PMXCrossover) };
            yield return new object[] { nameof(DoNothingCrossover) };
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenNoOffspringIsSameInstanceAsAParent(string crossoverName)
        {
            var parent1 = PermutationTestData.CreateTestChromosome1();
            var parent2 = PermutationTestData.CreateTestChromosome2();
            var crossover = Create(crossoverName);

            var offspring = crossover.Cross(parent1, parent2);

            offspring.Should().NotBeEmpty();
            foreach (var child in offspring)
            {
                child.Should().NotBeSameAs(parent1);
                child.Should().NotBeSameAs(parent2);
            }
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenNoOffspringSharesAGeneArrayWithAParent(string crossoverName)
        {
            var parent1 = PermutationTestData.CreateTestChromosome1();
            var parent2 = PermutationTestData.CreateTestChromosome2();
            var crossover = Create(crossoverName);

            var offspring = crossover.Cross(parent1, parent2);

            foreach (PermutationChromosome child in offspring)
            {
                child.Genes.Should().NotBeSameAs(parent1.Genes);
                child.Genes.Should().NotBeSameAs(parent2.Genes);
            }
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenParentGenesAreUnchanged(string crossoverName)
        {
            var parent1 = PermutationTestData.CreateTestChromosome1();
            var parent2 = PermutationTestData.CreateTestChromosome2();
            var before1 = (int[])parent1.Genes.Clone();
            var before2 = (int[])parent2.Genes.Clone();
            var crossover = Create(crossoverName);

            crossover.Cross(parent1, parent2);

            parent1.Genes.Should().Equal(before1);
            parent2.Genes.Should().Equal(before2);
        }

        private static ICrossover Create(string crossoverName)
        {
            var random = new FastRandom(20260929);
            var parameterSet = new ParameterSet();
            var conflictDetectors = new List<IConflictDetector> { AllRightConflictDetector.Instance };

            return crossoverName switch
            {
                nameof(Order1Crossover) => new Order1Crossover(random, parameterSet, conflictDetectors),
                nameof(PMXCrossover) => new PMXCrossover(random, parameterSet, conflictDetectors),
                nameof(DoNothingCrossover) => new DoNothingCrossover(random, parameterSet, conflictDetectors),
                _ => throw new KeyNotFoundException(crossoverName)
            };
        }
    }
}
