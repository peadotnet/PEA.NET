using System;
using System.Linq;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetReplaceGeneMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetReplaceGeneMutation(IRandom random, ParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();

            if (chromosome.Sections.Length < 2) return chromosome;

            if (chromosome.ConflictList.Any())
            {
                var reducingConflictPossibility = ParameterSet.GetValue(ParameterNames.ConflictReducingPossibility);
                var rnd = Random.GetDouble(0, 1);
                if (rnd < reducingConflictPossibility)
                {
                    chromosome = ReplaceConflicted(chromosome);
                    return chromosome;
                }
            }

            chromosome = ReplaceRandom(chromosome);
            return chromosome;
        }

        private SortedSubsetChromosome ReplaceRandom(SortedSubsetChromosome chromosome)
        {
            var sourceSectionIndex = Random.GetInt(0, chromosome.Sections.Length);
            var sourcePosition = Random.GetInt(0, chromosome.Sections[sourceSectionIndex].Length);
            chromosome = Replace(chromosome, sourceSectionIndex, sourcePosition);
            return chromosome;
        }

        private SortedSubsetChromosome ReplaceConflicted(SortedSubsetChromosome chromosome)
        {
            var conflicted = Random.GetInt(0, chromosome.ConflictList.Count);
            var sourceSectionIndex = chromosome.ConflictList[conflicted].Key;
            var sourcePosition = chromosome.ConflictList[conflicted].Value;
            chromosome = Replace(chromosome, sourceSectionIndex, sourcePosition);
            return chromosome;
        }

        private SortedSubsetChromosome Replace(SortedSubsetChromosome chromosome, int sourceSectionIndex, int sourcePosition)
        {
            var geneValue = chromosome.Sections[sourceSectionIndex][sourcePosition];
            var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, sourceSectionIndex);

            var targetPos = FindNewGenePosition(chromosome, targetSectionIndex, geneValue);
            InsertGenes(chromosome, targetSectionIndex, targetPos, chromosome.Sections[sourceSectionIndex], sourcePosition, 1);
            DeleteGenesFromSection(chromosome, sourceSectionIndex, sourcePosition, 1);

            return chromosome;
        }
    }
}
