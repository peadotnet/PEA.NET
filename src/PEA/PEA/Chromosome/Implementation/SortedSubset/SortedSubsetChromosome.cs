using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetChromosome : IChromosome, IDeepCloneable<SortedSubsetChromosome>
    {
        public int[][] Sections { get; set; }

        public List<KeyValuePair<int, int>> ConflictList { get; } = new List<KeyValuePair<int, int>>();

        private SortedSubsetChromosome() { }

        public SortedSubsetChromosome(ICollection<ICollection<int>> sections) : this()
        {
            if (sections == null) throw new ArgumentNullException(nameof(sections));

            Sections = new int[sections.Count][];

            var cIdx = 0;
            foreach (var section in sections)
            {
                if (section == null) throw new ArgumentNullException(nameof(sections) + "[" + cIdx + "]");

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
