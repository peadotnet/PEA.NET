using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class ReplaceOneGeneMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public ReplaceOneGeneMutation(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
            : base(random, parameterSet, conflictDetector)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();
            if (chromosome.Sections.Length < 2) return null;

            var source = GetSourceSectionAndPosition(chromosome);
            ReplaceOneGeneToRandomSection(chromosome, source);

            return chromosome;
        }
    }
}
