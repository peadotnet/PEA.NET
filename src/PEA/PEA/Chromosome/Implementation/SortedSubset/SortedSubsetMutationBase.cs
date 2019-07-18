using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public abstract class SortedSubsetMutationBase : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        protected SortedSubsetMutationBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {
        }

        public abstract SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome);

        public virtual IChromosome Mutate(IChromosome chromosome)
        {
            var sortedSubsetChromosome = chromosome as SortedSubsetChromosome;
            if (sortedSubsetChromosome == null) throw new ArgumentException(nameof(chromosome));

            return Mutate(sortedSubsetChromosome);
        }

        /// <summary>
        /// Inserts one gene value into a randomly choosen section and deletes it from its original position
        /// </summary>
        /// <param name="chromosome"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public bool ReplaceOneGeneToRandomSection(SortedSubsetChromosome chromosome, GenePosition source)
        {
            var geneValue = chromosome.Sections[source.Section][source.Position];
            var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
            var targetSection = chromosome.Sections[targetSectionIndex];

            var targetPos = FindNewGenePosition(targetSection, geneValue);

            //TODO: conflict check, fail retry
            var success = InsertGenes(chromosome, targetSectionIndex, targetPos, chromosome.Sections[source.Section], source.Position, 1);

            if (success) DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);

            return success;
        }
    }
}
