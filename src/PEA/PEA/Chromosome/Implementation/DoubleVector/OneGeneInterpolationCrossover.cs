using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
	public class OneGeneInterpolationCrossover : DoubleVectorOperatorBase, ICrossover<DoubleVectorChromosome>
	{
        public OneGeneInterpolationCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public IList<IChromosome> Cross(IChromosome iparent0, IChromosome iparent1)
        {
            var children = new List<IChromosome>();

            var parent0 = iparent0 as DoubleVectorChromosome;
            var parent1 = iparent1 as DoubleVectorChromosome;
            var length = parent0.Genes.Length;

            double[] child0;
            double[] child1;

            bool child0Conflicted = false;
            bool child1Conflicted = false;

            int position = Random.GetInt(0, length);
            double weight = Random.GetDouble(0, 1);

            child0 = MergeGenes(parent0.Genes, parent1.Genes, position, weight, ref child0Conflicted);
            child1 = MergeGenes(parent1.Genes, parent0.Genes, position, weight, ref child1Conflicted);

            var child0Chromosome = new DoubleVectorChromosome(child0);
            children.Add(child0Chromosome);
            var child1Chromosome = new DoubleVectorChromosome(child1);
            children.Add(child1Chromosome);

            return children;
        }

        public double[] MergeGenes(double[] parent0Genes, double[] parent1Genes, int position, double weight, ref bool conflicted)
        {
            if (conflicted) return null;

            var newValue = weight * parent0Genes[position] + (1 - weight) * parent1Genes[position];

   //         if (ConflictDetected(position, newValue))
			//{
   //             conflicted = true;
   //             return null;
			//}

            double[] childGenes = new double[parent0Genes.Length];

            for (int i = 0; i < parent0Genes.Length; i++)
            {
                childGenes[i] = i == position ? newValue : parent0Genes[i];
            }

            return childGenes;
        }
    }
}
