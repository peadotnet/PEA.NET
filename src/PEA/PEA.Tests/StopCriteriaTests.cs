using FluentAssertions;
using NSubstitute;
using Pea.Core;
using Pea.StopCriteria;
using Pea.StopCriteria.Implementation;
using System;
using Xunit;

namespace Pea.Tests
{
    public class StopCriteriaTests
    {
        [Fact]
        public void StopCriteriaBuilder_BuildEmpty_ShouldReturnDefault()
        {
            var builder = StopCriteriaBuilder.StopWhen();
            var result = builder.Build();
            result.Should().BeOfType<TimeOutStopCriteria>();
        }

        [Fact]
        public void StopCriteriaBuilder_AddSingleCriteria_ShouldReturnCriteria()
        {
            var criteria = new TimeOutStopCriteria();
            var builder = StopCriteriaBuilder.StopWhen()
                .When(criteria);

            var result = builder.Build();

            result.Should().Be(criteria);
        }

        [Fact]
        public void StopCriteriaBuilder_BuildComplex_ShouldReturnTree()
        {
            var fitness = Substitute.For<IFitness>();

            var builder = StopCriteriaBuilder.StopWhen()
                .TimeoutElapsed(100000)
                .And().FitnessLimitExceeded(fitness)
                .Or().TimeoutElapsed(600000);

            var result = builder.Build();

            result.Should().BeOfType<OrStopCriteria>();
            var resultOr = result as OrStopCriteria;
            resultOr.Criteria2.Should().BeOfType<TimeOutStopCriteria>();
            resultOr.Criteria1.Should().BeOfType<AndStopCriteria>();
            var resultAnd = resultOr.Criteria1 as AndStopCriteria;
            resultAnd.Criteria1.Should().BeOfType<TimeOutStopCriteria>();
            resultAnd.Criteria2.Should().BeOfType<FitnessLimitExceededStopCriteria>();
        }
    }
}
    