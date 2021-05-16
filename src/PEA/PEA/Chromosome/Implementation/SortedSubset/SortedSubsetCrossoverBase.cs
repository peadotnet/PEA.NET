using System;
using System.Collections.Generic;
using System.Text;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public abstract class SortedSubsetCrossoverBase : SortedSubsetOperatorBase, ICrossover<SortedSubsetChromosome>
    {
        protected SortedSubsetCrossoverBase(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors) : base(random, parameterSet, conflictDetectors)
        {
        }

        public abstract IList<IChromosome> Cross(IChromosome iparent0, IChromosome iparent1);

        public static int[] GetParentSection(SortedSubsetChromosome chromosome, int sectionIndex)
        {
            bool sectionExists = (chromosome.Sections.Length > sectionIndex);
            var section = sectionExists ? chromosome.Sections[sectionIndex] : new int[0];
            return section;
        }

        public GeneRange GetParentRange(int[] section, int crossoverPointLeft, int crossoverPointRight, int sectionIndex)
        {
            var positionLeft = FindNewGenePosition(section, crossoverPointLeft);
            var positionRight = FindNewGenePosition(section, crossoverPointRight);
            var range1 = new GeneRange(sectionIndex, positionLeft, positionRight);
            return range1;
        }
    }
}
