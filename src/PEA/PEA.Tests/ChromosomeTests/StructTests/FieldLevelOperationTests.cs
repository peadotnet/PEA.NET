using FluentAssertions;
using Pea.Chromosome;
using Pea.Chromosome.Implementation.StructVector.FieldLevelOperations;
using Pea.Configuration.Implementation;
using Pea.Core;
using System.Collections.Generic;
using Xunit;

namespace Pea.Tests.ChromosomeTests.StructTests
{
	public struct TestStruct
	{
		public double DoubleField { get; set; }
		public bool BitField { get; set; }
		public int IntegerField { get; set; }
	}

	public class FieldLevelOperationTests
	{
		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void BitFieldOperator_Get_ShouldReturn(bool expected)
		{
			var testStruct = new TestStruct()
			{
				DoubleField = 0,
				BitField = expected,
				IntegerField = 0
			};

			var random = new FastRandom(20210728);
			var parameterSet = new ParameterSet();
			var fieldOperator = new BitFieldOperator<TestStruct>("BitField", random, parameterSet);

			var result = (bool)fieldOperator.GetValue(testStruct);

			result.Should().Be(expected);
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void BitFieldOperator_Set_ShouldSet(bool expected)
		{
			var testStruct = new TestStruct()
			{
				DoubleField = 0.0,
				BitField = !expected,
				IntegerField = 0
			};

			var random = new FastRandom(20210728);
			var parameterSet = new ParameterSet();
			var fieldOperator = new BitFieldOperator<TestStruct>("BitField", random, parameterSet);

			object testObject = testStruct;
			fieldOperator.SetValue(testObject, expected);

			((TestStruct)testObject).BitField.Should().Be(expected);
		}

		[Theory]
		[InlineData(2.0, 6.0)]
		public void DoubleFieldOperator_Set_ShouldSet(double change, double expected)
		{
			var testStruct = new TestStruct()
			{
				DoubleField = 4.0,
				BitField = false,
				IntegerField = 0
			};

			var random = new PredeterminedRandom(new double[] { 1, 1 });
			var parameterSet = new ParameterSet(new List<PeaSettingsNamedValue>() { new PeaSettingsNamedValue(ParameterNames.MutationIntensity, change) });
			var fieldOperator = new DoubleFieldOperator<TestStruct>("DoubleField", random, parameterSet);

			fieldOperator.Mutate(testStruct);


		}
	}
}
