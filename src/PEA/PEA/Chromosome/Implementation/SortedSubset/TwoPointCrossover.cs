using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class TwoPointCrossover : SortedSubsetCrossoverBase
    {
        public TwoPointCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
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
            var child1 = new int[sectionsCount][];
            bool child0Conflicted = false;
            bool child1Conflicted = false;

            int retryCount = ParameterSet.GetInt(ParameterNames.FailedCrossoverRetryCount);
            while (true)
            {
                var crossoverPointLeft = Random.GetInt(0, totalCount);
                var crossoverPointRight = Random.GetIntWithTabu(0, totalCount, crossoverPointLeft);

                if (crossoverPointRight < crossoverPointLeft)
                {
                    var temp = crossoverPointLeft;
                    crossoverPointLeft = crossoverPointRight;
                    crossoverPointRight = temp;
                }

                for (int sectionIndex = 0; sectionIndex < sectionsCount; sectionIndex++)
                {
                    bool section0Exists = (parent1.Sections.Length > sectionIndex);
                    var section0 = section0Exists ? parent1.Sections[sectionIndex] : new int[0];
                    var position0left = section0Exists ? FindNewGenePosition(section0, crossoverPointLeft) : 0;
                    var position0right = section0Exists ? FindNewGenePosition(section0, crossoverPointRight) : 0;

                    bool section1Exists = (parent2.Sections.Length > sectionIndex);
                    var section1 = section1Exists ? parent2.Sections[sectionIndex] : new int[0];
                    var position1left = section1Exists ? FindNewGenePosition(section1, crossoverPointLeft) : 0;
                    var position1right = section1Exists ? FindNewGenePosition(section1, crossoverPointRight) : 0;

                    var child0Section = MergeSections(section0, position0left, position0right, section1, position1left, position1right, ref child0Conflicted);
                    child0[sectionIndex] = child0Section;

                    var child1Section = MergeSections(section1, position1left, position1right, section0, position0left, position0right, ref child1Conflicted);
                    child1[sectionIndex] = child1Section;

                    //TODO: Merge conflict lists

                    if (child0Conflicted && child1Conflicted) break;
                }

                if (!child0Conflicted || !child1Conflicted)
                {
                    var conflictedPositions0 =
                        child0Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child0);
                    var conflictedPositions1 =
                        child1Conflicted ? new List<GenePosition>() : SortedSubsetChromosomeValidator.SearchForConflict(child1);

                    if (conflictedPositions0.Count > 0 || conflictedPositions1.Count > 0)
                    {
                        bool error = true;  //For breakpoint
                    }

                    break;
                }

                if(retryCount-- < 0) break;

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
    }
}
