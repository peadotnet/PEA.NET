using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PrecedenceMatrixModel : PermutationOperatorBase, IProbabilisticModel<PermutationChromosome>
    {
        public double[,] PrecedenceMatrix { get; private set; }

        public PrecedenceMatrixModel(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public void Learn(IList<IChromosome> chromosomes)
        {
            for (int i = 0; i < chromosomes.Count; i++)
            {
                Add(chromosomes[i]);
            }
        }

        public void Remove(IChromosome chromosome)
        {
            var genes = ((PermutationChromosome)chromosome).Genes;
            RecalculateMatrix(genes, -1);
        }

        public void Add(IChromosome chromosome)
        {
            var genes = ((PermutationChromosome)chromosome).Genes;
            RecalculateMatrix(genes, 1);
        }

        public void RecalculateMatrix(int[] genes, int sign)
        {
            var length = genes.Length;

            if (PrecedenceMatrix == null) PrecedenceMatrix = new double[length, length];

            if (PrecedenceMatrix.GetLength(0) != genes.Length) throw new ApplicationException("The size of the chromosome is different from the probabilistic model.");

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var difference = Difference(i, j, length);
                    if (difference > 0)
                    {
                        PrecedenceMatrix[i, j] += sign / difference;
                    }
                }
            }
        }

        public int Difference(int position1, int position2, int length)
        {
            var diff = position1 - position2;
            if (diff < 0) diff += length;

            return diff - length / 2;
        }

        public IChromosome GetSample()
        {
            var length = PrecedenceMatrix.GetLength(0);
            var precedences = new double[length];

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    precedences[i] += PrecedenceMatrix[i, j];
                }
            }

            var result = new PermutationChromosome();
            return result;
        }

    }
}
