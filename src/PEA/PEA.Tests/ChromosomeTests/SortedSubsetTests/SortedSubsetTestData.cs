using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;

namespace Pea.Tests.ChromosomeTests
{
    public class SortedSubsetTestData
    {
        public static SortedSubsetChromosome CreateChromosome()
        {
            var section1 = new int[] { 1, 4, 5, 7 };
            var section2 = new int[] { 3, 6, 8 };
            var section3 = new int[] { 2, 9 };

            var sections = new int[][]
            {
                section1, section2, section3
            };

            var chromosome = new SortedSubsetChromosome(sections);

            return chromosome;
        }

        public static SortedSubsetChromosome CreateOtherChromosome()
        {
            var section1 = new int[] { 3, 4, 9 };
            var section2 = new int[] { 2, 5, 7 }; 
            var section3 = new int[] { 1, 6, 8 }; 

            var sections = new int[][]
            {
                section1, section2, section3
            };

            var chromosome = new SortedSubsetChromosome(sections);

            return chromosome;
        }

        public static IList<IChromosome> CreateChromosomes()
        {
            return new List<IChromosome>()
            {
                CreateChromosome(),
                CreateOtherChromosome()
            };
        }
    }
}
