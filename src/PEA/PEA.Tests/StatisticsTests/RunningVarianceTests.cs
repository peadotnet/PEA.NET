using FluentAssertions;
using Pea.Util;
using System;
using Xunit;

namespace Pea.Tests.StatisticsTests
{
	public class RunningVarianceTests
	{
		[Fact]
		public void RunningVariance_AddSameValueRepeatedly_ShouldNotChange()
		{
			var runningVariance = new RunningVariance();
			runningVariance.Add(42.0);
			runningVariance.Add(42.0);
			runningVariance.Add(42.0);

			runningVariance.Mean.Should().Be(42.0);
			runningVariance.Deviation.Should().Be(0);
		}

		[Fact]
		public void RunningVariance_Remove_ShouldRemoved()
		{
			var runningVariance = new RunningVariance();
			runningVariance.Add(Math.PI);
			runningVariance.Add(-1 * Math.E);
			runningVariance.Add(42.0);
			runningVariance.Add(42.0);
			runningVariance.Add(42.0);
			runningVariance.Add(42.0);
			runningVariance.Remove(Math.PI);
			runningVariance.Remove(-1 * Math.E);

			runningVariance.Mean.Should().Be(42.0);
			runningVariance.Deviation.Should().BeApproximately(0, 0.000001);
		}

		[Fact]
		public void RunningVariance_WithMultipleValue_WhenReplaceAll_ShouldeProper()
		{
			var runningVariance = new RunningVariance();
			for(int i=0; i< 100; i++)
			{
				runningVariance.Add(84.0);
			}

			for(int j=0; j < 100; j++)
			{
				runningVariance.Add(42.0);
				runningVariance.Remove(84.0);
			}

			runningVariance.Mean.Should().BeApproximately(42.0, 0.00001);
			runningVariance.Deviation.Should().BeApproximately(0, 0.00001);
		}

		[Fact]
		public void RunningVariance_AddLessValueRepeatedly_ShouldDecreasing()
		{
			var runningVariance = new RunningVariance();
			runningVariance.Add(84.0);
			runningVariance.Add(42.0);

			var mean = runningVariance.Mean;
			var deviation = runningVariance.Deviation;

			for(int i = 0; i < 10; i++)
			{
				runningVariance.Add(42.0);
				runningVariance.Mean.Should().BeLessThan(mean);
				runningVariance.Deviation.Should().BeLessThan(deviation);

				mean = runningVariance.Mean;
				deviation = runningVariance.Deviation;
			}
		}
	}
}
