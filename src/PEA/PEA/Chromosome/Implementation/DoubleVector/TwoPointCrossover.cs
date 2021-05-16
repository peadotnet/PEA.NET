﻿using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class TwoPointCrossover : DoubleVectorOperatorBase, ICrossover<DoubleVectorChromosome>
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IList<IChromosome> Cross(IChromosome iparent0, IChromosome iparent1)
        {
            var children = new List<IChromosome>();

            var parent0 = iparent0 as DoubleVectorChromosome;
            var parent1 = iparent1 as DoubleVectorChromosome;
            var length = parent0.Genes.Length;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            if (length / blockSize < 3) return children;

            double[] child0 = new double[length];
            double[] child1 = new double[length];

            bool child0Conflicted = false;
            bool child1Conflicted = false;

            while (true)
            {
                var blockPosition1 = Random.GetInt(1, length / blockSize);
                var blockPosition2 = Random.GetIntWithTabu(1, length / blockSize, blockPosition1);

                var crossoverPosition1 = blockPosition1 * blockSize;
                var crossoverPosition2 = blockPosition2 * blockSize;

                SwapPositions(ref crossoverPosition1, ref crossoverPosition2);

                child0 = MergeGenes(parent0.Genes, parent1.Genes, crossoverPosition1, crossoverPosition2, ref child0Conflicted);
                child1 = MergeGenes(parent1.Genes, parent0.Genes, crossoverPosition1, crossoverPosition2, ref child1Conflicted);

                if (!child0Conflicted || !child1Conflicted)
                {
                    break;
                }
            }

            if (!child0Conflicted)
            {
                var child0Chromosome = new DoubleVectorChromosome(child0);
                children.Add(child0Chromosome);
            }

            if (!child1Conflicted)
            {
                var child1Chromosome = new DoubleVectorChromosome(child1);
                children.Add(child1Chromosome);
            }

            return children;
        }

        private static void SwapPositions(ref int crossoverPosition1, ref int crossoverPosition2)
        {
            if (crossoverPosition1 > crossoverPosition2)
            {
                int temp = crossoverPosition1;
                crossoverPosition1 = crossoverPosition2;
                crossoverPosition2 = temp;
            }
        }

        public double[] MergeGenes(double[] parent0Genes, double[] parent1Genes, int crossoverPosition1, int crossoverPosition2, ref bool childConflicted)
        {
            //TODO: ConflictDetectors
            double[] childGenes = new double[parent0Genes.Length];

            Array.Copy(parent0Genes, 0, childGenes, 0, crossoverPosition1);

            var middleLendth = crossoverPosition2 - crossoverPosition1;
            Array.Copy(parent1Genes, crossoverPosition1, childGenes, crossoverPosition1, middleLendth);

            var rigthLength = parent0Genes.Length - crossoverPosition2;
            Array.Copy(parent0Genes, crossoverPosition2, childGenes, crossoverPosition2, rigthLength);

            return childGenes;
        }
    }
}
