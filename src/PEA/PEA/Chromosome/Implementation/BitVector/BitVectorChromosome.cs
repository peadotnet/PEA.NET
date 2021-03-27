using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class BitVectorChromosome : IChromosome, IDeepCloneable<BitVectorChromosome>
	{
        public bool[] Genes { get; set; }

        public BitVectorChromosome(ICollection<bool> genes)
        {
            Genes = new bool[genes.Count];
            genes.CopyTo(Genes, 0);
        }

        public BitVectorChromosome DeepClone()
        {
            return new BitVectorChromosome(Genes);
        }

        IChromosome IDeepCloneable<IChromosome>.DeepClone()
        {
            return DeepClone();
        }
    }
}
