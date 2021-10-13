﻿using FluentAssertions;
using Pea.Core;
using Xunit;

namespace Pea.Tests.CoreTests
{
    public class StochasticProviderTests
    {
        [Fact]
        public void EmptyProbabilityDistribution_ChooseOne_ShouldDefault()
        {
            var distribution = new StochasticProvider<string>(new PredeterminedRandom(20));

            var result = distribution.GetOne();

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void NonEmptyProbabilityDistribution_ChooseOne_ShouldReturnOne()
        {
            var random = new PredeterminedRandom(80, 30);
            var distribution = new StochasticProvider<string>(random)
                .Add("First", 50)
                .Add("Second", 40);

            distribution.GetOne().Should().Be("Second");
            distribution.GetOne().Should().Be("First");
        }
    }
}
