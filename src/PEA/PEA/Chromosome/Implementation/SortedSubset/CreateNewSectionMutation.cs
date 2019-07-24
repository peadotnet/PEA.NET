using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class CreateNewSectionMutation : SortedSubsetMutationBase
    {
        public CreateNewSectionMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();

            var numberOfGenesToReplace = GetNumberOfGenesToChange(chromosome);
            chromosome = IncrementNumberOfSections(chromosome, numberOfGenesToReplace);
            var targetSection = chromosome.Sections.Length - 1;
            var target = chromosome.Sections[targetSection];

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);

            for (int i = 0; i < numberOfGenesToReplace; i++)
            {
                bool replaced = false;
                var replaceCount = retryCount;
                while (true)
                {
                    GenePosition source = GetSourceSectionAndPosition(chromosome);
                    var geneValue = chromosome.Sections[source.Section][source.Position];
                    var targetPos = FindNewGenePosition(target, geneValue);
                    if (!ConflictDetectedWithLeftNeighbor(target, targetPos, geneValue)
                        && ConflictDetectedWithRightNeighbor(target, targetPos, geneValue))
                    {
                        InsertGenes(chromosome, targetSection, targetPos, chromosome.Sections[source.Section], source.Position, 1);
                        DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);
                        replaced = true;
                    }

                    if (replaced || replaceCount-- < 0) break;
                }
            }

            CleanOutSections(chromosome);
            return chromosome;
        }

        private int GetNumberOfGenesToChange(SortedSubsetChromosome chromosome)
        {
            int max = chromosome.TotalCount / chromosome.Sections.Length;
            int min = Math.Min(chromosome.ConflictList.Count, max);

            return Random.GetInt(min, max);
        }

        public SortedSubsetChromosome IncrementNumberOfSections(SortedSubsetChromosome chromosome, int numberOfGenesToReplace)
        {
            int length = chromosome.Sections.Length;
            var newSections = new int[length + 1][];
            Array.Copy(chromosome.Sections, newSections, length);

            newSections[length] = new int[numberOfGenesToReplace];

            chromosome.Sections = newSections;
            return chromosome;
        }
    }
}
