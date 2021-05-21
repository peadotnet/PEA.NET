using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pea.Tests.FitnessTests
{
	public struct MultiObjectiveFitness
	{
		public double[] Value { get; }
		public double[] OriginalValue { get; set; }

		public MultiObjectiveFitness(params double[] values)
		{
			Value = values;
			OriginalValue = null;
		}
	}

	public class StructureFitnessTests
	{
	}
}
