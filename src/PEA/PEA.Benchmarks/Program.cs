using BenchmarkDotNet.Running;
using PEA.Benchmarks.ChromosomeBenchmarks;
using PEA.Benchmarks.CoreBenchmarks;

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
