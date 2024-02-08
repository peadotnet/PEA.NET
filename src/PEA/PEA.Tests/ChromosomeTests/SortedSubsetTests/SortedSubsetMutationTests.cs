﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Pea.Chromosome;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;
using Xunit;

namespace Pea.Tests.ChromosomeTests.SortedSubsetTests
{
    public class SortedSubsetMutationTests
    {
        [Fact]
        public void GivenChromosome_WithOneSection_WhenReplaceOneGene_ThenShouldReturnNull()
        {
            var random = new SystemRandom(Environment.TickCount);
            var parameterSet = new ParameterSet();
            var chromosome = new SortedSubsetChromosome(new List<ICollection<int>>() {new int[] {1, 2, 3, 4, 5, 6}});
            var conflictDetectors = new List<INeighborhoodConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new ReplaceOneGeneMutation(random, parameterSet, conflictDetectors);

            var result = mutation.Mutate(chromosome);

            result.Should().BeNull();
        }

        [Fact]
        public void GivenChromosome_WithConflict_WhenReplaceOneGene_ThenShouldBeReplaceConflicted()
        {
            var random = new PredeterminedRandom(0.5, 3, 1); //possibility, conflictIndex, targetSection
            var parameterSet = new ParameterSet();
            parameterSet.SetValue(Chromosome.ParameterNames.ConflictReducingProbability, 0.6);
            parameterSet.SetValue(Chromosome.ParameterNames.FailedMutationRetryCount, 0);
            var chromosome = SortedSubsetTestData.CreateChromosome();
            chromosome.ConflictList.Add(new GenePosition(0, 1));
            var conflictDetectors = new List<INeighborhoodConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new ReplaceOneGeneMutation(random, parameterSet, conflictDetectors);

            var result = mutation.Mutate(chromosome);

            result.Sections[0].Should().BeEquivalentTo(new int[] {1, 5, 7});
            result.Sections[1].Should().BeEquivalentTo(new int[] {3, 4, 6, 8});
            result.Sections[2].Should().BeEquivalentTo(new int[] {2, 9});
        }

        [Fact]
        public void GivenChromosome_WithConflict_WhenReplaceOneGene_ThenShouldBeReplaceRandom()
        {
            var random = new PredeterminedRandom(0.7, 1, 2, 1, 2); //possibility, sourceSection, sourcePosition, wrongTargetSection, goodTargetSection
            var parameterSet = new ParameterSet();
            parameterSet.SetValue(Chromosome.ParameterNames.ConflictReducingProbability, 0.6);
            parameterSet.SetValue(Chromosome.ParameterNames.FailedMutationRetryCount, 0);
            var chromosome = SortedSubsetTestData.CreateChromosome();
            chromosome.ConflictList.Add(new GenePosition(0, 1));
            var conflictDetectors = new List<INeighborhoodConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new ReplaceOneGeneMutation(random, parameterSet, conflictDetectors);

            var result = mutation.Mutate(chromosome);

            result.Sections[0].Should().BeEquivalentTo(new int[] { 1, 4, 5, 7 });
            result.Sections[1].Should().BeEquivalentTo(new int[] { 3, 6 });
            result.Sections[2].Should().BeEquivalentTo(new int[] { 2, 8, 9 });
        }

        [Fact]
        public void GivenChromosome_WithoutConflict_ThenShouldBeReplaceRandom()
        {
            var random = new PredeterminedRandom(1, 2, 1, 2); //possibility, sourceSection, sourcePosition, wrongTargetSection, goodTargetSection
            var parameterSet = new ParameterSet();
            parameterSet.SetValue(Chromosome.ParameterNames.FailedMutationRetryCount, 0);

            var chromosome = SortedSubsetTestData.CreateChromosome();
            var conflictDetectors = new List<INeighborhoodConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new ReplaceOneGeneMutation(random, parameterSet, conflictDetectors);

            var result = mutation.Mutate(chromosome);

            result.Sections[0].Should().BeEquivalentTo(new int[] { 1, 4, 5, 7 });
            result.Sections[1].Should().BeEquivalentTo(new int[] { 3, 6 });
            result.Sections[2].Should().BeEquivalentTo(new int[] { 2, 8, 9 });
        }
    }
}
