using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PMXCrossover : PermutationCrossoverBase
    {
        public PMXCrossover(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null) : base(random, parameterSet, conflictDetector)
        {

        }

        public override IList<PermutationChromosome> Cross(IList<PermutationChromosome> parents)
        {
            var swapRange = GetSourceRange(parents[0]);

            var parent1Genes = parents[0].Genes;
            var parent2Genes = parents[1].Genes;
            var length = parent1Genes.Length;

            var child1Genes = new int[length];
            var child2Genes = new int[length];

            Dictionary<int, int> swap1 = new Dictionary<int, int>();
            Dictionary<int, int> swap2 = new Dictionary<int, int>();
            for (int pos = swapRange.Position; pos < swapRange.Position + swapRange.Length; pos++)
            {
                swap1.Add(parent2Genes[pos], pos);
                swap2.Add(parent1Genes[pos], pos);
            }

            if (swapRange.Position > 0)
            {
                CopyWithDuplicationElimination(swapRange, parent1Genes, child1Genes, swap1, 0, swapRange.Position);
                CopyWithDuplicationElimination(swapRange, parent2Genes, child2Genes, swap2, 0, swapRange.Position);
            }

            Array.Copy(parent1Genes, swapRange.Position, child2Genes, swapRange.Position, swapRange.Length);
            Array.Copy(parent2Genes, swapRange.Position, child1Genes, swapRange.Position, swapRange.Length);

            int swapEnd = swapRange.Position + swapRange.Length;
            if (swapEnd  < parent1Genes.Length)
            {
                CopyWithDuplicationElimination(swapRange, parent1Genes, child1Genes, swap1, swapEnd, length - swapEnd);
                CopyWithDuplicationElimination(swapRange, parent2Genes, child2Genes, swap2, swapEnd, length - swapEnd);
            }

            var child1 = new PermutationChromosome(child1Genes);
            var child2 = new PermutationChromosome(child2Genes);

            var result = new List<PermutationChromosome>()
            {
                child1,
                child2
            };
            return result;
        }

        private static void CopyWithDuplicationElimination(GeneRange swapRange, int[] parent2Genes, int[] child1Genes, Dictionary<int, int> swap1, int begin, int end)
        {
            for (int pos = begin; pos < end; pos++)
            {
                int geneValue1 = GetUniqueGeneValue(parent2Genes, swap1, pos);
                child1Genes[pos] = geneValue1;
            }
        }

        private static int GetUniqueGeneValue(int[] parent2Genes, Dictionary<int, int> swap1, int pos)
        {
            var geneValue = parent2Genes[pos];
            while (swap1.ContainsKey(geneValue))
            {
                geneValue = parent2Genes[swap1[geneValue]];
            }

            return geneValue;
        }
    }
}
