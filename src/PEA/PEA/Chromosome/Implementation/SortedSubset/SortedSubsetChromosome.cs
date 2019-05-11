using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetChromosome : IChromosome, IDeepCloneable<SortedSubsetChromosome>
    {
        public int TotalCount { get; }

        public int[][] Sections { get; set; }

        public List<GeneRange> ConflictList { get; } = new List<GeneRange>();

        private SortedSubsetChromosome() { }

        public SortedSubsetChromosome(ICollection<ICollection<int>> sections) : this()
        {
            if (sections == null) throw new ArgumentNullException(nameof(sections));

            TotalCount = 0;

            Sections = new int[sections.Count][];

            var cIdx = 0;
            foreach (var section in sections)
            {
                if (section == null) throw new ArgumentNullException(nameof(sections) + "[" + cIdx + "]");

                TotalCount += section.Count;

                Sections[cIdx] = new int[section.Count];
                section.CopyTo(Sections[cIdx], 0);
                cIdx++;

            }
        }

        public SortedSubsetChromosome DeepClone()
        {
            var clone = new SortedSubsetChromosome(this.Sections);
            return clone;
        }
    }
}
