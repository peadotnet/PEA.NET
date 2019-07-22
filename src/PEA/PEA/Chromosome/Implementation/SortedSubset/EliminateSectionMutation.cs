using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class EliminateSectionMutation : SortedSubsetMutationBase
    {
        public EliminateSectionMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();
            if (chromosome.Sections.Length < 2) return null;

            SortedSubsetChromosome clone;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);

            clone = chromosome.DeepClone();
            var sectionToEliminate = Random.GetInt(0, clone.Sections.Length);
            for (int g = 0; g < clone.Sections[sectionToEliminate].Length; g++)
            {
                var geneValue = clone.Sections[sectionToEliminate][g];
                var replaceCount = retryCount;

                while (true)
                {
                    var targetSection = Random.GetIntWithTabu(0, clone.Sections.Length, sectionToEliminate);
                    var targetPosition = FindNewGenePosition(clone.Sections[targetSection], geneValue);

                    if (!ConflictDetectedWithLeftNeighbor(clone.Sections[targetSection], targetPosition, geneValue)
                        && !ConflictDetectedWithRightNeighbor(clone.Sections[targetSection], targetPosition, geneValue))
                    {
                        //TODO: simplify this method parameter set! (or make an alternative one)
                        InsertGenes(clone, targetSection, targetPosition, new int[] {geneValue}, 0, 1);
                        DeleteGenesFromSection(clone, sectionToEliminate, g, 1);
                        break;
                    }

                    if (replaceCount-- < 0) break;
                }
            }

            return clone;
        }
    }
}
