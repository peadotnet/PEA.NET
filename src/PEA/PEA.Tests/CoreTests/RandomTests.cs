using System;
using System.Collections.Generic;
using FluentAssertions;
using Pea.Core;
using Xunit;

namespace Pea.Tests.CoreTests
{
    public class RandomTests
    {
        [Theory]
        [MemberData(nameof(IntTestData))]
        public void GivenPredeterminedRandom_WhenGetIntWithMinMax_ThenShouldReturnProper(int minValue, int maxValue)
        {
            var random = new PredeterminedRandom(100, 101, 102, 103);
            TestRandomGetInt(random, minValue, maxValue);
        }

        [Theory]
        [MemberData(nameof(IntDoubleData))]
        public void GivenPredeterminedRandom_WhenGetDoubleWithMinMax_ThenShouldReturnProper(double minValue, double maxValue)
        {
            var random = new PredeterminedRandom(100.17, 101.33, 3.141592654, 2.71828);
            TestRandomGetDouble(random, minValue, maxValue);
        }

        [Theory]
        [MemberData(nameof(IntTestData))]
        public void GivenFastRandom_WhenGetIntWithMinMax_ThenShouldReturnProper(int minValue, int maxValue)
        {
            var random = new FastRandom();
            TestRandomGetInt(random, minValue, maxValue);
        }

        [Theory]
        [MemberData(nameof(IntDoubleData))]
        public void GivenFastRandom_WhenGetDoubleWithMinMax_ThenShouldReturnProper(double minValue, double maxValue)
        {
            var random = new FastRandom();
            TestRandomGetDouble(random, minValue, maxValue);
        }

        [Theory]
        [MemberData(nameof(IntTestData))]
        public void GivenSystemRandom_WhenGetIntWithMinMax_ThenShouldReturnProper(int minValue, int maxValue)
        {
            var random = new SystemRandom();
            TestRandomGetInt(random, minValue, maxValue);
        }

        [Theory]
        [MemberData(nameof(IntDoubleData))]
        public void GivenSystemRandom_WhenGetDoubleWithMinMax_ThenShouldReturnProper(double minValue, double maxValue)
        {
            var random = new SystemRandom();
            TestRandomGetDouble(random, minValue, maxValue);
        }

        [Fact]
        public void GivenRandom_WhenGetIntWithTabu_ThenShouldReturnDifferent()
        {
            var random = new PredeterminedRandom(10, 10, 25, 30);
            var result = random.GetIntWithTabu(0, 50, 10);
            result.Should().Be(25);
        }

        private void TestRandomGetInt(IRandom random, int minValue, int maxValue)
        {
            for (int i = 0; i < 5; i++)
            {
                var result = random.GetInt(minValue, maxValue);
                result.Should().BeGreaterOrEqualTo(minValue);
                result.Should().BeLessOrEqualTo(maxValue);
            }
        }

        private void TestRandomGetDouble(IRandom random, double minValue, double maxValue)
        {
            for (int i = 0; i < 6; i++)
            {
                var result = random.GetDouble(minValue, maxValue);
                result.Should().BeGreaterOrEqualTo(minValue);
                result.Should().BeLessOrEqualTo(maxValue);
            }
        }

        public static IEnumerable<object[]> IntTestData =>
            new List<object[]>()
            {
                new object[] {1, 1},
                new object[] {5, 5},
                new object[] {0, 6},
                new object[] {5, 6},
            };

        public static IEnumerable<object[]> IntDoubleData =>
            new List<object[]>()
            {
                new object[] {1.0, 1.0},
                new object[] {5.0, 5.0},
                new object[] {0.0, 6.0},
                new object[] {5.17, 6.91},
                new object[] {0.0, 32767.35},
            };

    }
}
