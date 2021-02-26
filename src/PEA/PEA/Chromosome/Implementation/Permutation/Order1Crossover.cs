using System;
using System.Collections.Generic;
using System.Text;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class Order1Crossover : PermutationCrossoverBase
    {
        public Order1Crossover(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null) 
            : base(random, parameterSet, conflictDetectors)
        {
        }

        public override IList<IChromosome> Cross(IList<IChromosome> parents)
        {
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

            HashSet<int> swapRangeGenes = new HashSet<int>();
            for(int i=swapRange.Position; i <= swapRange.End; i++)
            {
                swapRangeGenes.Add(parent1Genes[i]);
            }

            int pos = 0;
            for (int p = 0; p < length; p++)
            {
                if (p >= swapRange.Position && p <= swapRange.End)
                {
                    childGenes[p] = parent1Genes[p];
                }
                else
                {
                    while (swapRangeGenes.Contains(parent2Genes[pos])) pos++;

                    childGenes[p] = parent2Genes[pos];
                    pos++;
                }
            }
            return childGenes;
        }
    }
}
