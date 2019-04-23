using FluentAssertions;
using Pea.Core;
using Xunit;

namespace Pea.Tests.CoreTests
{
    public class StochasticProviderTests
    {
        [Fact]
        public void GivenEmptyProbabilityDistribution_WhenChooseOne_ThenShouldDefault()
        {
            var distribution = new StochasticProvider<string>(new PredeterminedRandom(20));

            var result = distribution.ChooseOne();

            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public void GivenNonEmptyProbabilityDistribution_WhenChooseOne_ThenShouldReturnOne()
        {
            var random = new PredeterminedRandom(80, 30);
            var distribution = new StochasticProvider<string>(random)
                .Add("First", 50)
                .Add("Second", 40);

            distribution.ChooseOne().Should().Be("Second");
            distribution.ChooseOne().Should().Be("First");
        }
    }
}
