using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class CreateNewSectionMutation : SortedSubsetMutationBase
    {
        public CreateNewSectionMutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) return null;

            var numberOfGenesToReplace = GetNumberOfGenesToChange(chromosome);

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedMutationRetryCount);
            int replaced = 0;

            var targetList = new LinkedList<int>();

            for (int i = 0; i < numberOfGenesToReplace; i++)
            {
                var tryCount = retryCount;

                while (true)
                {
                    GenePosition source = GetSourceSectionAndPosition(chromosome);
                    var geneValue = chromosome.Sections[source.Section][source.Position];
                    var success = InsertGeneToLinkedList(targetList, geneValue);
                    if (success)
                    {
                        DeleteGenesFromSection(chromosome, source.Section, source.Position, 1);
                        replaced++;
                    }

                    if (success || tryCount-- < 0) break;
                }
            }

            IncrementNumberOfSections(chromosome, targetList);

            CleanOutSections(chromosome);

            return chromosome;
        }

        private int GetNumberOfGenesToChange(SortedSubsetChromosome chromosome)
        {
            int max = chromosome.TotalCount / chromosome.Sections.Length;
            int min = Math.Min(chromosome.ConflictList.Count, max);

            return Random.GetInt(min, max);
        }

        public bool InsertGeneToLinkedList(LinkedList<int> list, int geneValue)
        {
            if (list.First == null)
            {
                list.AddFirst(geneValue);
                return true;
            }

            if (list.First.Value > geneValue)
            {
                if (ConflictDetectors[0].ConflictDetected(geneValue, list.First.Value)) return false;

                list.AddFirst(geneValue);
                return true;
            }

            if (list.Last.Value < geneValue)
            {
                if (ConflictDetectors[0].ConflictDetected(list.Last.Value, geneValue)) return false; //TODO: multiple detectors!

                list.AddLast(geneValue);
                return true;
            }

            var node = list.First;
            while (node.Value < geneValue) node = node.Next;

            if (ConflictDetectors[0].ConflictDetected(node.Previous.Value, geneValue) ||
                ConflictDetectors[0].ConflictDetected(geneValue, node.Value)) //TODO: multiple detectors!
            {
                return false;
            }

            list.AddBefore(node, geneValue);
            return true;
        }

        public SortedSubsetChromosome IncrementNumberOfSections(SortedSubsetChromosome chromosome, LinkedList<int> sectionList)
        {
            int length = chromosome.Sections.Length;
            var newSections = new int[length + 1][];
            Array.Copy(chromosome.Sections, newSections, length);

            newSections[length] = new int[sectionList.Count];
            var node = sectionList.First;
            for (int i = 0; i < sectionList.Count; i++)
            {
                newSections[length][i] = node.Value;
                node = node.Next;
            }

            chromosome.Sections = newSections;
            return chromosome;
        }
    }
}
