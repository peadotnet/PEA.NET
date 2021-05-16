using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class OnePointCrossover : DoubleVectorOperatorBase, ICrossover<DoubleVectorChromosome>
    {
        public OnePointCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IList<IChromosome> Cross(IChromosome iparent0, IChromosome iparent1)
        {
            var children = new List<IChromosome>();

            var parent0 = iparent0 as DoubleVectorChromosome;
            var parent1 = iparent1 as DoubleVectorChromosome;
            var length = parent0.Genes.Length;

            double[] child0 = new double[length];
            double[] child1 = new double[length];

            bool child0Conflicted = false;
            bool child1Conflicted = false;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);
            int blockSize = ParameterSet.GetInt(ParameterNames.BlockSize);

            while (true)
            {

                var blockPosition = Random.GetInt(1, length / blockSize);
                var crossoverPosition = blockPosition * blockSize;

                child0 = MergeGenes(parent0.Genes, parent1.Genes, crossoverPosition, ref child0Conflicted);
                child1 = MergeGenes(parent1.Genes, parent0.Genes, crossoverPosition, ref child1Conflicted);


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

        public double[] MergeGenes(double[] parent0Genes, double[] parent1Genes, int crossoverPosition, ref bool childConflicted)
        {
            //TODO: ConflictDetectors

            double[] childGenes = new double[parent0Genes.Length];

            Array.Copy(parent0Genes, 0, childGenes, 0, crossoverPosition);

            var rigthLength = parent1Genes.Length - crossoverPosition;
            Array.Copy(parent1Genes, crossoverPosition, childGenes, crossoverPosition, rigthLength);

            return childGenes;
        }
    }
}
