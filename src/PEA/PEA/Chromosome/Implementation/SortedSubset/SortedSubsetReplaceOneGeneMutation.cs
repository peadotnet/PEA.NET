using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetReplaceOneGeneMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetReplaceOneGeneMutation(IRandom random, IParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();
            if (chromosome.Sections.Length < 2) return null;

            var source = GetSourceSectionAndPosition(chromosome);
            return Replace(chromosome, source);
        }

        private GeneRegion GetSourceSectionAndPosition(SortedSubsetChromosome chromosome)
        {
            return ConflictShouldBeEliminated(chromosome)
                ? GetConflictedSectionAndPosition(chromosome)
                : GetRandomSectionAndPosition(chromosome);
        }

        public SortedSubsetChromosome Replace(SortedSubsetChromosome chromosome, GenePosition source)
        {
            var geneValue = chromosome.Sections[source.Section][source.Position];
            var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);

            var targetPos = FindNewGenePosition(chromosome, targetSectionIndex, geneValue);
            InsertGenes(chromosome, targetSectionIndex, targetPos, chromosome.Sections[source.Section], source.Position, 1);
            DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);

            return chromosome;
        }
    }
}
