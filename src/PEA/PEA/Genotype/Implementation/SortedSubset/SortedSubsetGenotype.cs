using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Genotype.Implementation.SortedSubset
{
    public class SortedSubsetGenotype : IDeepCloneable<SortedSubsetGenotype>
    {
        public int[][] Chromosomes { get; set; }

        public List<KeyValuePair<int, int>> ConflictList { get; } = new List<KeyValuePair<int, int>>();

        private SortedSubsetGenotype() { }

        public SortedSubsetGenotype(ICollection<ICollection<int>> chromosomes) : this()
        {
            if (chromosomes == null) throw new ArgumentNullException(nameof(chromosomes));

            Chromosomes = new int[chromosomes.Count][];

            var cIdx = 0;
            foreach (var chrom in chromosomes)
            {
                if (chrom == null) throw new ArgumentNullException(nameof(chromosomes) + "[" + cIdx + "]");

                Chromosomes[cIdx] = new int[chrom.Count];
                chrom.CopyTo(Chromosomes[cIdx], 0);
                cIdx++;
            }
        }

        public SortedSubsetGenotype DeepClone()
        {
            var clone = new SortedSubsetGenotype(this.Chromosomes);
            return clone;
        }
    }
}
