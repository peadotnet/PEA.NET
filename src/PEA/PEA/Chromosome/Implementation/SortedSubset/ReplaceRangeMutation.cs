using Pea.Core;
using System;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class ReplaceRangeMutation : SortedSubsetMutationBase
    {
        public ReplaceRangeMutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Sections.Length < 2) return null;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            while (true)
            {
                var source = GetSourceSectionAndPosition(chromosome);
                var section = chromosome.Sections[source.Section];
                var position = source.Position;

                var length = Random.GetInt(0, section.Length - source.Position);

                var targetSectionIndex = Random.GetIntWithTabu(0, chromosome.Sections.Length, source.Section);
                var targetSection = chromosome.Sections[targetSectionIndex];

                while (position < source.Position + length)
                {
                    var geneValue = section[position];
                    var targetPosition = FindNewGenePosition(targetSection, geneValue);
                    var insertable = CountInsertableGenes(chromosome, targetSectionIndex, targetPosition, section, position);

                    bool conflict = ConflictDetectedWithLeftNeighbor(targetSection, targetPosition, geneValue);
                    if (!conflict)
                    {
                        var lastGeneValue = section[position + insertable - 1];
                        conflict = ConflictDetectedWithRightNeighbor(targetSection, targetPosition, lastGeneValue);
                    }

                    if (conflict) break;

                    InsertGenes(chromosome, targetSectionIndex, targetPosition, section, position, insertable);
                    position += insertable;
                }

                if (position > source.Position || retryCount-- < 0) break;
            }

            CleanOutSections(chromosome);
            return chromosome;
        }
    }
}
