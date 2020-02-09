using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class UniformInterpolationCrossover : DoubleVectorOperatorBase, ICrossover<DoubleVectorChromosome>
    {
        public UniformInterpolationCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var children = new List<IChromosome>();

            var parent0 = parents[0] as DoubleVectorChromosome;
            var parent1 = parents[1] as DoubleVectorChromosome;
            var length = parent0.Genes.Length;


            double[] child0 = new double[length];
            double[] child1 = new double[length];

            bool child0Conflicted = false;
            bool child1Conflicted = false;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);

            while (true)
            {
                child0 = MergeGenes(parent0.Genes, parent1.Genes);
                child1 = MergeGenes(parent1.Genes, parent0.Genes);

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

        public double[] MergeGenes(double[] parent0Genes, double[] parent1Genes)
        {
            //TODO: ConflictDetectors
            double[] childGenes = new double[parent0Genes.Length];

            for (int i = 0; i < parent0Genes.Length; i++)
            {
                var weight = Random.GetDouble(0, 1);
                childGenes[i] = weight * parent0Genes[i] + (1 - weight) * parent1Genes[i];
            }

            return childGenes;
        }

    }
}
