using System.Collections.Generic;
using FluentAssertions;
using Pea.Chromosome.Implementation.DoubleVector;
using Pea.Core;
using Xunit;
using ChromosomeParameterNames = Pea.Chromosome.ParameterNames;

namespace Pea.Tests.ChromosomeTests.DoubleVectorTests
{
    public class DoubleVectorCrossoverContractTests
    {
        public static IEnumerable<object[]> Crossovers()
        {
            yield return new object[] { nameof(DoNothingCrossover) };
            yield return new object[] { nameof(InterpolationCrossover) };
            yield return new object[] { nameof(OneGeneInterpolationCrossover) };
            yield return new object[] { nameof(OnePointCrossover) };
            yield return new object[] { nameof(TwoPointCrossover) };
            yield return new object[] { nameof(UniformCrossover) };
            yield return new object[] { nameof(UniformInterpolationCrossover) };
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

            foreach (DoubleVectorChromosome child in offspring)
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
            var before1 = (double[])parent1.Genes.Clone();
            var before2 = (double[])parent2.Genes.Clone();

            Create(crossoverName).Cross(parent1, parent2);

            parent1.Genes.Should().Equal(before1);
            parent2.Genes.Should().Equal(before2);
        }

        private static DoubleVectorChromosome CreateParent1() =>
            new DoubleVectorChromosome(new[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0 });

        private static DoubleVectorChromosome CreateParent2() =>
            new DoubleVectorChromosome(new[] { -1.5, -2.5, -3.5, -4.5, -5.5, -6.5, -7.5, -8.5, -9.5, -10.5 });

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
                nameof(InterpolationCrossover) => new InterpolationCrossover(random, parameterSet, conflictDetectors),
                nameof(OneGeneInterpolationCrossover) => new OneGeneInterpolationCrossover(random, parameterSet, conflictDetectors),
                nameof(OnePointCrossover) => new OnePointCrossover(random, parameterSet, conflictDetectors),
                nameof(TwoPointCrossover) => new TwoPointCrossover(random, parameterSet, conflictDetectors),
                nameof(UniformCrossover) => new UniformCrossover(random, parameterSet, conflictDetectors),
                nameof(UniformInterpolationCrossover) => new UniformInterpolationCrossover(random, parameterSet, conflictDetectors),
                _ => throw new KeyNotFoundException(crossoverName)
            };
        }
    }
}
