using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PermutationChromosome : IChromosome, IDeepCloneable<PermutationChromosome>
    {
        public int[] Genes { get; set; }

        public PermutationChromosome() { }

        public PermutationChromosome(ICollection<int> genes)
        {
            Genes = new int[genes.Count];
            genes.CopyTo(Genes, 0);
        }

        public PermutationChromosome DeepClone()
        {
            return new PermutationChromosome(Genes);
        }

        IChromosome IDeepCloneable<IChromosome>.DeepClone()
        {
            return DeepClone();
        }
    }
}
