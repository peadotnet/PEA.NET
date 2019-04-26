using System;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetCreateNewSectionMutation : SortedSubsetOperatorBase, IMutation<SortedSubsetChromosome>
    {
        public SortedSubsetCreateNewSectionMutation(IRandom random, IParameterSet parameterSet) : base(random, parameterSet)
        {
        }

        public SortedSubsetChromosome Mutate(SortedSubsetChromosome chromosome)
        {
            if (chromosome == null) throw new ArgumentNullException();

            var numberOfGenesToReplace = GetNumberOfGenesToChange(chromosome);
            chromosome = IncrementNumberOfSections(chromosome, numberOfGenesToReplace);

            //TODO: folyt köv innen

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
