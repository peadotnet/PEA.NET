using System.Collections.Generic;
using Pea.Chromosome.Implementation.Permutation;

namespace Pea.Tests.ChromosomeTests.PermutationTests
{
    public class PermutationTestData
    {
        public static IList<PermutationChromosome> CreateTestChromosomes()
        {
            var chromosome1 = CreateTestChromosome1();
            var chromosome2 = CreateTestChromosome2();

            var result = new List<PermutationChromosome>() { chromosome1, chromosome2 };
            return result;
        }

        public static PermutationChromosome CreateTestChromosome1()
        {
            var genes1 = new int[] { 7, 2, 8, 4, 1, 6, 3, 5, 9, 0 };
            var chromosome1 = new PermutationChromosome(genes1);
            return chromosome1;
        }

        public static PermutationChromosome CreateTestChromosome2()
        {
            var genes2 = new int[] { 4, 5, 9, 0, 3, 8, 2, 7, 6, 1 };
            var chromosome2 = new PermutationChromosome(genes2);
            return chromosome2;
        }

    }
}
