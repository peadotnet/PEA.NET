using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PMXCrossover : PermutationCrossoverBase
    {
        public PMXCrossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
        {

        }

        public override IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            //TODO: ConflictDetection, repeat, 
            var result = new List<IChromosome>();

            var parent1 = parents[0] as PermutationChromosome;
            var parent2 = parents[1] as PermutationChromosome;

            var swapRange = GetSourceRange(parent1);

            int[] child1Genes = CrossoverGenes(parent1.Genes, parent2.Genes, swapRange);
            var child1 = new PermutationChromosome(child1Genes);
            result.Add(child1);

            int[] child2Genes = CrossoverGenes(parent2.Genes, parent1.Genes, swapRange);
            var child2 = new PermutationChromosome(child2Genes);
            result.Add(child2);

            return result;
        }

        public int[] CrossoverGenes(int[] parent1Genes, int[] parent2Genes, GeneRange swapRange)
        {
            var length = parent1Genes.Length;
            var childGenes = new int[length];

            var swapRegionGeneMap = GenerateGeneMap(parent1Genes, swapRange);

            if (swapRange.Position > 0)
            {
                CopyWithDuplicationElimination(parent2Genes, childGenes, swapRegionGeneMap, 0, swapRange.Position);
            }

            Array.Copy(parent1Genes, swapRange.Position, childGenes, swapRange.Position, swapRange.Length);

            int swapEnd = swapRange.Position + swapRange.Length;
            if (swapEnd < parent2Genes.Length)
            {
                CopyWithDuplicationElimination(parent2Genes, childGenes, swapRegionGeneMap, swapEnd, length);
            }

            return childGenes;
        }

        public Dictionary<int, int> GenerateGeneMap(int[] parent2Genes, GeneRange swapRange)
        {
            var swapRegionGeneMap = new Dictionary<int, int>();
            for (int pos = swapRange.Position; pos < swapRange.Position + swapRange.Length; pos++)
            {
                swapRegionGeneMap.Add(parent2Genes[pos], pos);
            }

            return swapRegionGeneMap;
        }

        public void CopyWithDuplicationElimination(int[] parentGenes, int[] childGenes, Dictionary<int, int> geneMap, int begin, int end)
        {
            for (int pos = begin; pos < end; pos++)
            {
                int geneValue = GetUniqueGeneValue(parentGenes, geneMap, pos);
                childGenes[pos] = geneValue;
            }
        }

        public int GetUniqueGeneValue(int[] genes, Dictionary<int, int> geneMap, int position)
        {
            var geneValue = genes[position];
            while (geneMap.ContainsKey(geneValue))
            {
                geneValue = genes[geneMap[geneValue]];
            }

            return geneValue;
        }
    }
}
