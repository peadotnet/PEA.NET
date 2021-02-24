using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class ShuffleRangeMutation : PermutationMutationBase
    {
        public ShuffleRangeMutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors = null) : base(random, parameterSet, conflictDetectors)
        {
        }


        public override PermutationChromosome Mutate(PermutationChromosome chromosome)
        {
            if (chromosome == null) return null;
            if (chromosome.Genes.Length < 2) return null;

            var genes = chromosome.Genes;
            var range = GetSourceRange(chromosome);
            var rangeStart = range.Position;
            var rangeEnd = range.Position + range.Length - 1;

            Shuffle(genes, rangeStart, rangeEnd);

            return chromosome;

        }

        public void Shuffle(int[] genes, int rangeStart, int rangeEnd)
        {
            for (int pos = rangeEnd; pos > rangeStart; pos--)
            {
                var mutationProbability = 0.2; // ParameterSet.GetValue(ParameterNames.MutationProbability);
                var rnd = Random.GetDouble(0, 1);
                if (mutationProbability > rnd)
                {
                    var pos2 = Random.GetInt(0, pos);
                    var tmp = genes[pos];
                    genes[pos] = genes[pos2];
                    genes[pos2] = tmp;
                }
            }
        }
    }
}
