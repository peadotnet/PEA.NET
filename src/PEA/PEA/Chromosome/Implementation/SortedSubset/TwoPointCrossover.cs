using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class TwoPointCrossover : SortedSubsetCrossoverBase
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            var children = new List<IChromosome>();

            var parent1 = parents[0] as SortedSubsetChromosome;
            var parent2 = parents[1] as SortedSubsetChromosome;

            var sectionsCount = parent1.Sections.Length > parent2.Sections.Length
                ? parent1.Sections.Length
                : parent2.Sections.Length;

            var totalCount = parent1.TotalCount > parent2.TotalCount
                ? parent1.TotalCount
                : parent2.TotalCount;

            var child0 = new int[sectionsCount][];
            bool child0Conflicted = false;

            var child1 = new int[sectionsCount][];
            bool child1Conflicted = false;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);
            while (true)
            {
                var crossoverPointLeft = Random.GetInt(0, totalCount);
                var crossoverPointRight = Random.GetIntWithTabu(0, totalCount, crossoverPointLeft);
                SortIncrementalOrder(ref crossoverPointLeft, ref crossoverPointRight);

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    int[] section1 = GetParentSection(parent1, sectionIndex);
                    var range1 = GetParentRange(section1, crossoverPointLeft, crossoverPointRight, sectionIndex);

                    var section2 = GetParentSection(parent2, sectionIndex);
                    var range2 = GetParentRange(section2, crossoverPointLeft, crossoverPointRight, sectionIndex);

                    var child0Section = MergeSections(section1, range1, section2, range2, ref child0Conflicted);
                    child0[sectionIndex] = child0Section;

                    var child1Section = MergeSections(section2, range2, section1, range1, ref child1Conflicted);
                    child1[sectionIndex] = child1Section;

                    //TODO: Merge conflict lists

                    if (child0Conflicted && child1Conflicted) break;
                }

                if (!child0Conflicted || !child1Conflicted)
                {
                    //TODO: Delete this
                    var conflictedPositions0 =
                        child0Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child0);
                    var conflictedPositions1 =
                        child1Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child1);

                    if (conflictedPositions0.Count > 0 || conflictedPositions1.Count > 0)
                    {
                        bool error = true;  //For breakpoint
                        throw new ApplicationException("Conflict between neighboring values! (Crossover: TwoPointCrossover)");
                    }

                    break;
                }

                if (retryCount-- < 0) break;

                child0 = new int[sectionsCount][];
                child1 = new int[sectionsCount][];
                child0Conflicted = false;
                child1Conflicted = false;
            }

            if (!child0Conflicted)
            {
                var child0Chromosome = new SortedSubsetChromosome(child0);
                CleanOutSections(child0Chromosome);
                children.Add(child0Chromosome);
            }

            if (!child1Conflicted)
            {
                var child1Chromosome = new SortedSubsetChromosome(child1);
                CleanOutSections(child1Chromosome);
                children.Add(child1Chromosome);
            }

            return children;
        }

        public static void SortIncrementalOrder(ref int crossoverPointLeft, ref int crossoverPointRight)
        {
            if (crossoverPointRight < crossoverPointLeft)
            {
                var temp = crossoverPointLeft;
                crossoverPointLeft = crossoverPointRight;
                crossoverPointRight = temp;
            }
        }
    }
}
