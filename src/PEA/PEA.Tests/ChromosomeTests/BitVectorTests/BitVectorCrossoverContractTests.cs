using System.Collections.Generic;
using FluentAssertions;
using Pea.Chromosome.Implementation.BitVector;
using Pea.Core;
using Xunit;
using ChromosomeParameterNames = Pea.Chromosome.ParameterNames;

namespace Pea.Tests.ChromosomeTests.BitVectorTests
{
    public class BitVectorCrossoverContractTests
    {
        public static IEnumerable<object[]> Crossovers()
        {
            yield return new object[] { nameof(DoNothingCrossover) };
            yield return new object[] { nameof(TwoPointCrossover) };
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenNoOffspringIsSameInstanceAsAParent(string crossoverName)
        {
            var parent1 = CreateParent1();
            var parent2 = CreateParent2();

            var offspring = Create(crossoverName).Cross(parent1, parent2);

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
            var parent1 = CreateParent1();
            var parent2 = CreateParent2();

            var offspring = Create(crossoverName).Cross(parent1, parent2);

            foreach (BitVectorChromosome child in offspring)
            {
                child.Genes.Should().NotBeSameAs(parent1.Genes);
                child.Genes.Should().NotBeSameAs(parent2.Genes);
            }
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenParentGenesAreUnchanged(string crossoverName)
        {
            var parent1 = CreateParent1();
            var parent2 = CreateParent2();
            var before1 = (bool[])parent1.Genes.Clone();
            var before2 = (bool[])parent2.Genes.Clone();

            Create(crossoverName).Cross(parent1, parent2);

            parent1.Genes.Should().Equal(before1);
            parent2.Genes.Should().Equal(before2);
        }

        private static BitVectorChromosome CreateParent1() =>
            new BitVectorChromosome(new[] { true, true, true, true, true, false, false, false, false, false });

        private static BitVectorChromosome CreateParent2() =>
            new BitVectorChromosome(new[] { false, true, false, true, false, true, false, true, false, true });

        private static ICrossover Create(string crossoverName)
        {
            var random = new FastRandom(20261007);
            var parameterSet = new ParameterSet();
            parameterSet.SetValue(ChromosomeParameterNames.FailedCrossoverRetryCount, 3, ParameterSource.UserSetting);
            parameterSet.SetValue(ChromosomeParameterNames.BlockSize, 1, ParameterSource.UserSetting);
            var conflictDetectors = new List<IConflictDetector> { AllRightConflictDetector.Instance };

            return crossoverName switch
            {
                nameof(DoNothingCrossover) => new DoNothingCrossover(random, parameterSet, conflictDetectors),
                nameof(TwoPointCrossover) => new TwoPointCrossover(random, parameterSet, conflictDetectors),
                _ => throw new KeyNotFoundException(crossoverName)
            };
        }
    }
}
