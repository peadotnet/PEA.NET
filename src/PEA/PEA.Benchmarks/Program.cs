using BenchmarkDotNet.Running;

namespace PEA.Benchmarks
{
	class Program
    {
        // Run all:            dotnet run -c Release
        // Run one:            dotnet run -c Release -- --filter *PopulationIndexer*
        // List available:     dotnet run -c Release -- --list flat
        //
        // Results land in BenchmarkDotNet.Artifacts/results/ .
        // The full JSON reports there are what ResultsComparer consumes when comparing two runs:
        //   dotnet run --project ResultsComparer -- --base <old-results-dir> --diff <new-results-dir> --threshold 2%
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
