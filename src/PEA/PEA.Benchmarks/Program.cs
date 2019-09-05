using BenchmarkDotNet.Running;
using PEA.Benchmarks.ChromosomeBenchmarks;

namespace PEA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PermutationOperatorBenchmarks>();
        }
    }
}
