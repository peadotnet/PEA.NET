using System;
using BenchmarkDotNet.Attributes;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;
using System.Collections.Generic;
using System.Linq;

namespace PEA.Benchmarks.ChromosomeBenchmarks
{
    [MinColumn, MaxColumn]
    [HtmlExporter, RPlotExporter]
    public class PermutationOperatorBenchmarks
    {
        [Params(100)]
        public int Count { get; set; }

        [Params(52, 104)]
        public int Size { get; set; }

        public IRandom Random = new FastRandom(DateTime.Now.Millisecond);
        public ParameterSet ParameterSet = new ParameterSet();

        List<IChromosome> Chromosomes = new List<IChromosome>();

        private PrecedenceMatrixModel PrebuildModel;


        public PermutationOperatorBenchmarks()
        {
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var chromosomeCreator = new PermutationRandomCreator(Size, Random, conflictDetectors);
            for (int i = 0; i < Count; i++)
            {
                Chromosomes.Add(chromosomeCreator.Create());
            }

            PrebuildModel = new PrecedenceMatrixModel(Random, ParameterSet);
            PrebuildModel.Add(Chromosomes);
        }

        public List<IChromosome> MutateChromosomes(IMutation mutation)
        {
            var result = new List<IChromosome>();
            for (int i = 0; i < Chromosomes.Count; i++)
            {
                result.Add(mutation.Mutate(Chromosomes[i]));
            }
            return result;
        }

        [Benchmark]
        public double[,] BuildModel()
        {
            var precedenceMatrixModel = new PrecedenceMatrixModel(Random, ParameterSet);
            precedenceMatrixModel.Add(Chromosomes);
            return precedenceMatrixModel.PrecedenceMatrix;
        }

        [Benchmark]
        public List<IChromosome> GetSamples()
        {
            var result = new List<IChromosome>();

            var precedenceMatrixModel = new PrecedenceMatrixModel(Random, ParameterSet);

            if (Count != Chromosomes.Count) throw new ApplicationException($"Mi a f... {Count} / {Chromosomes.Count}");

            for (int i = 0; i < Chromosomes.Count; i++)
            {
                precedenceMatrixModel.Add(new List<IChromosome>() { Chromosomes[i] });
                var chromosome = precedenceMatrixModel.GetSample();
                result.Add(chromosome);
            }

            return result;
        }

        [Benchmark]
        public List<IChromosome> RelocateRange()
        {
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new RelocateRangeMutation(Random, ParameterSet, conflictDetectors);
            return MutateChromosomes(mutation);
        }

        [Benchmark]
        public List<IChromosome> InverseRange()
        {
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new InverseRangeMutation(Random, ParameterSet, conflictDetectors);
            return MutateChromosomes(mutation);
        }

        [Benchmark]
        public List<IChromosome> SwapTwoRange()
        {
            var conflictDetectors = new List<IConflictDetector>() { AllRightConflictDetector.Instance };
            var mutation = new SwapTwoRangeMutation(Random, ParameterSet, conflictDetectors);
            return MutateChromosomes(mutation);
        }
    }
}
