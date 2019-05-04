using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetOnePointCrossover : SortedSubsetOperatorBase, ICrossover<SortedSubsetChromosome>
    {
        public SortedSubsetOnePointCrossover(IRandom random, IParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public IList<SortedSubsetChromosome> Cross(IList<SortedSubsetChromosome> parents)
        {
            var children = new List<SortedSubsetChromosome>();
            var sectionsCount = parents[0].Sections.Length > parents[1].Sections.Length ? parents[0].Sections.Length : parents[1].Sections.Length;
            var totalCount = parents[0].TotalCount > parents[1].TotalCount ? parents[0].TotalCount : parents[1].TotalCount;
            var crossoverPosition = Random.GetInt(0, totalCount);

            var child0 = new SortedSubsetChromosome(new int[sectionsCount][]);
            var child1 = new SortedSubsetChromosome(new int[sectionsCount][]);

            for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
            {
                var position0 = FindNewGenePosition(parents[0], sectionIndex, crossoverPosition);
                var position1 = FindNewGenePosition(parents[1], sectionIndex, crossoverPosition);
                var remaining0 = parents[0].Sections[sectionIndex].Length - position0;
                var remaining1 = parents[1].Sections[sectionIndex].Length - position1;

                var child0Section = new int[position0 + remaining1];
                var child1Section = new int[position1 + remaining0];

                Array.Copy(parents[0].Sections[sectionIndex], 0, child0Section, 0, position0);
                Array.Copy(parents[1].Sections[sectionIndex], position1, child0Section, position1, remaining1);

                Array.Copy(parents[1].Sections[sectionIndex], 0, child1Section, 0, position1);
                Array.Copy(parents[0].Sections[sectionIndex], position0, child1Section, position0, remaining0);

                child0.Sections[sectionIndex] = child0Section;
                child1.Sections[sectionIndex] = child1Section;

                //TODO: continue! Zero length check, conflict check! Fail Retry!
            }

            return children;
        }
    }
}
