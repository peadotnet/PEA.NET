using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace PEA.Benchmarks.CoreBenchmarks
{
	[MinColumn, MaxColumn]
	//[HtmlExporter, RPlotExporter]
	public class ImmutableArrayBenchmarks
	{
		private const int N = 10000;

		int[] Array;

		ImmutableArray<int> Immutable;

		[Params(5, 20, 100, 500)]
		public int Size { get; set; }

		[GlobalSetup]
		public void Setup()
		{
			var array = new int[N];
			var immutable = ImmutableArray.Create<int>(array);
			Array = array;
			Immutable = immutable;
		}

		[Benchmark]
		public int ArrayOperation()
		{
			int value = 0;
			for(int n = 0; n < N; n++)
			{
				var array = new int[Array.Length];
				Array.CopyTo(array, 0);
				for(int i=0; i < Array.Length; i++)
				{
					array[i]++;
				}
				value = array[0];
				Array = array;
			}
			return value;
		}

		[Benchmark]
		public int ImmutableOperation()
		{
			int value = 0;
			for (int n = 0; n < N; n++)
			{
				var array = new int[Array.Length];
				Immutable.CopyTo(array, 0);
				for (int i = 0; i < Array.Length; i++)
				{
					array[i]++;
				}
				value = array[0];
				Immutable = ImmutableArray.Create<int>(array);
			}
			return value;
		}

	}
}
