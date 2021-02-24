using Pea.Core;
using Pea.Util;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PermutationUnivariateModel : PermutationOperatorBase, IProbabilisticModel
    {
        public RunningVariance[] Variances { get; private set; }

        protected PermutationUnivariateModel(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors = null) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public void Add(IChromosome chromosome)
        {
            var genes = ((PermutationChromosome)chromosome).Genes;
            int length = genes.Length;

            if (Variances == null) InitVariances(length);

            for (int i = 0; i < length; i++)
            {
                Variances[genes[i]].Add(i);
            }

        }

        private void InitVariances(int length)
        {
            Variances = new RunningVariance[length];
            for (int i = 0; i < length; i++)
            {
                Variances[i] = new RunningVariance();
            }
        }

        public void Remove(IChromosome chromosome)
        {
            if (Variances == null) throw new InvalidOperationException("The probabilistic model is uninitialized.");

            var genes = ((PermutationChromosome)chromosome).Genes;
            int length = genes.Length;

            for (int i = 0; i < length; i++)
            {
                Variances[genes[i]].Remove(i);
            }
        }

        public void Learn(IList<IChromosome> chromosomes)
        {
            Variances = null;

            for (int i = 0; i < chromosomes.Count; i++)
            {
                Add(chromosomes[i]);
            }
        }

        public IChromosome GetSample()
        {
            //TODO: Unit tests!
            var length = Variances.Length;
            var positions = new List<PositionValuePair<double>>();

            for (int i = 0; i < length; i++)
            {
                var mean = Variances[i].Mean;
                var deviation = Variances[i].Deviation;

                var position = Random.GetGaussian(mean, deviation);
                positions.Add(new PositionValuePair<double>(position, i));
            }

            var quickSorter = new QuickSorter<PositionValuePair<double>>();
            quickSorter.Sort(positions, PositionValuePair<double>.ComparerByPosition.Instance, 0, length-1);

            var genes = new int[length];

            for (int i = 0; i < length; i++)
            {
                genes[i] = positions[i].Value;
            }

            return new PermutationChromosome(genes);
        }
    }
}
