using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Xunit;
using ChromosomeParameterNames = Pea.Chromosome.ParameterNames;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class SortedSubsetCrossoverContractTests
    {
        public static IEnumerable<object[]> Crossovers()
        {
            yield return new object[] { nameof(OnePointCrossover) };
            yield return new object[] { nameof(TwoPointCrossover) };
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenNoOffspringIsSameInstanceAsAParent(string crossoverName)
        {
            var parent1 = SortedSubsetTestData.CreateChromosome();
            var parent2 = SortedSubsetTestData.CreateOtherChromosome();

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
        public void GivenTwoParents_WhenCross_ThenNoOffspringSharesASectionArrayWithAParent(string crossoverName)
        {
            var parent1 = SortedSubsetTestData.CreateChromosome();
            var parent2 = SortedSubsetTestData.CreateOtherChromosome();
            var parentArrays = new object[] { parent1.Sections, parent2.Sections }
                .Concat(parent1.Sections)
                .Concat(parent2.Sections)
                .ToList();

            var offspring = Create(crossoverName).Cross(parent1, parent2);

            foreach (SortedSubsetChromosome child in offspring)
            {
                foreach (var array in new object[] { child.Sections }.Concat(child.Sections))
                {
                    parentArrays.Should().NotContain(parentArray => ReferenceEquals(parentArray, array));
                }
            }
        }

        [Theory]
        [MemberData(nameof(Crossovers))]
        public void GivenTwoParents_WhenCross_ThenParentSectionsAreUnchanged(string crossoverName)
        {
            var parent1 = SortedSubsetTestData.CreateChromosome();
            var parent2 = SortedSubsetTestData.CreateOtherChromosome();
            var before1 = parent1.Sections.Select(section => section.ToArray()).ToArray();
            var before2 = parent2.Sections.Select(section => section.ToArray()).ToArray();

            Create(crossoverName).Cross(parent1, parent2);

            parent1.Sections.Should().BeEquivalentTo(before1, options => options.WithStrictOrdering());
            parent2.Sections.Should().BeEquivalentTo(before2, options => options.WithStrictOrdering());
        }

        private static ICrossover Create(string crossoverName)
        {
            var random = new FastRandom(20261007);
            var parameterSet = new ParameterSet();
            parameterSet.SetValue(ChromosomeParameterNames.FailedCrossoverRetryCount, 3, ParameterSource.UserSetting);
            var conflictDetectors = new List<INeighborhoodConflictDetector> { AllRightConflictDetector.Instance };

            return crossoverName switch
            {
                nameof(OnePointCrossover) => new OnePointCrossover(random, parameterSet, conflictDetectors),
                nameof(TwoPointCrossover) => new TwoPointCrossover(random, parameterSet, conflictDetectors),
                _ => throw new KeyNotFoundException(crossoverName)
            };
        }
    }
}
