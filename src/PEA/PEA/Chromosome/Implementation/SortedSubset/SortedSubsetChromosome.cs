using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetChromosome : IChromosome, IDeepCloneable<SortedSubsetChromosome>
    {
        public int TotalCount { get; }

        public int[][] Sections { get; set; }

        public List<GenePosition> ConflictList { get; } = new List<GenePosition>();

        public List<double> GeneratedRandoms = new List<double>();

        private SortedSubsetChromosome() { }

        public SortedSubsetChromosome(ICollection<ICollection<int>> sections) : this()
        {
            if (sections == null) throw new ArgumentNullException(nameof(sections));

            TotalCount = 0;

            //TODO: clone conflictList

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

        IChromosome IDeepCloneable<IChromosome>.DeepClone()
        {
            return DeepClone();
        }
    }
}
