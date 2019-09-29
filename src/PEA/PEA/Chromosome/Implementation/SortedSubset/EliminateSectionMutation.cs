using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class EliminateSectionMutation : SortedSubsetMutationBase
    {
        public EliminateSectionMutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Sections.Length < 2) return null;

            var sectionToEliminate = GetSectionToEliminate(chromosome);
            for (int g = chromosome.Sections[sectionToEliminate].Length - 1; g >= 0; g--)
            {
                var geneValue = chromosome.Sections[sectionToEliminate][g];
                var target = GetTargetSection(chromosome, sectionToEliminate, geneValue);

                if (target == null)
                {
                    continue;
                }

                InsertGenes(chromosome, target.Section, target.Position, new int[] {geneValue}, 0, 1);
                DeleteGenesFromSection(chromosome, sectionToEliminate, g, 1);
            }

            bool eliminated = CleanOutSections(chromosome);
            //TODO: Delete this
            //if (eliminated)
            //{
            //    var yepp = true;
            //}
            return chromosome;
        }

        private GenePosition GetTargetSection(SortedSubsetChromosome chromosome, int sectionToEliminate, int geneValue)
        {
            List<GenePosition> fitSections = new List<GenePosition>();
            for (int s = 0; s < chromosome.Sections.Length; s++)
            {
                if (s != sectionToEliminate)
                {
                    var position = FindNewGenePosition(chromosome.Sections[s], geneValue);
                    if (!ConflictDetectedWithLeftNeighbor(chromosome.Sections[s], position, geneValue)
                        && !ConflictDetectedWithRightNeighbor(chromosome.Sections[s], position, geneValue))
                    {
                        fitSections.Add(new GenePosition(s, position));
                    }
                }
            }

            if (fitSections.Count == 0) return null;

            var randomIndex = Random.GetInt(0, fitSections.Count);
            var chosenSection = fitSections[randomIndex];
            return chosenSection;
        }

        private int GetSectionToEliminate(SortedSubsetChromosome chromosome)
        {
            var provider = new StochasticProvider<int>(this.Random);
            for (int s = 0; s < chromosome.Sections.Length; s++)
            {
                var section = chromosome.Sections[s];
                var probability = chromosome.TotalCount / (double)section.Length;
                provider.Add(s, probability);
            }

            int chosen = provider.GetOne();
            return chosen;
        }
    }
}
