using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetReplaceOneGeneMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetReplaceOneGeneMutation(IRandom random, IParameterSet parameterSet) : base(random, parameterSet)
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
