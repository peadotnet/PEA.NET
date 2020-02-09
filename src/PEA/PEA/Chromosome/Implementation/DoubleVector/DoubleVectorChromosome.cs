using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.DoubleVector
{
    public class DoubleVectorChromosome : IChromosome, IDeepCloneable<DoubleVectorChromosome>
    {
        public double[] Genes { get; set; }

        public DoubleVectorChromosome(ICollection<double> genes)
        {
            Genes = new double[genes.Count];
            genes.CopyTo(Genes, 0);
        }

        public DoubleVectorChromosome DeepClone()
        {
            return new DoubleVectorChromosome(Genes);
        }

        IChromosome IDeepCloneable<IChromosome>.DeepClone()
        {
            return DeepClone();
        }
    }
}
