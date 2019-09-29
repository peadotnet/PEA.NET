using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public abstract class SortedSubsetMutationBase : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        protected SortedSubsetMutationBase(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public abstract SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome);

        public virtual IChromosome Mutate(IChromosome chromosome)
        {
            var sortedSubsetChromosome = chromosome as SortedSubsetChromosome;
            if (sortedSubsetChromosome == null) throw new ArgumentException(nameof(chromosome));

            return Mutate(sortedSubsetChromosome);
        }

        public GeneRange GetSourceRange(SortedSubsetChromosome chromosome)
        {
            var sourceStart = GetSourceSectionAndPosition(chromosome);
            var sourceLength = Random.GetInt(1, chromosome.Sections[sourceStart.Section].Length - sourceStart.Position + 1);
            var sourceEnd = sourceStart.Position + sourceLength;
            var range = new GeneRange(sourceStart.Section, sourceStart.Position, sourceEnd);
            return range;
        }

        /// <summary>
        /// Inserts one gene value into a randomly choosen section and deletes it from its original position
        /// </summary>
        /// <param name="chromosome"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool ReplaceOneGeneToRandomSection(SortedSubsetChromosome chromosome, GenePosition source, int retryCount)
        {
            var geneValue = chromosome.Sections[source.Section][source.Position];

            bool success = false;
            while (true)
            {
                var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
                var targetSection = chromosome.Sections[targetSectionIndex];
                var targetPos = FindNewGenePosition(targetSection, geneValue);

                success = InsertGenes(chromosome, targetSectionIndex, targetPos, 
                                            chromosome.Sections[source.Section], source.Position, 1);
                if (success) DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);

                if (success || retryCount-- < 0) break;
            }

            return success;
        }
    }
}
