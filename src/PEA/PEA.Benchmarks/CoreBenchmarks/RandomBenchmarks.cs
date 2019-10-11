using System;
using BenchmarkDotNet.Attributes;
using Pea.Core;

namespace PEA.Benchmarks.CoreBenchmarks
{
    public class RandomBenchmarks
    {
        private const int N = 10000;
        private IRandom _systemRandom = new SystemRandom(Environment.TickCount);
        private IRandom _fastRandom = new FastRandom(Environment.TickCount);

        private double[] GetRandomDouble(IRandom random)
        {
            double[] result = new double[N];
            for (int i = 0; i < N; i++)
            {
                result[i] = random.GetDouble(0, 1);
            }
            return result;
        }

        private int[] GetRandomInt(IRandom random)
        {
            int[] result = new int[N];
            for (int i = 0; i < N; i++)
            {
                result[i] = random.GetInt(0, 100000);
            }
            return result;
        }

        [Benchmark]
        public int[] SystemInt() => GetRandomInt(_systemRandom);

        [Benchmark]
        public int[] FastInt() => GetRandomInt(_fastRandom);

        [Benchmark]
        public double[] SystemDouble() => GetRandomDouble(_systemRandom);

        [Benchmark]
        public double[] FastDouble() => GetRandomDouble(_fastRandom);
    }
}
