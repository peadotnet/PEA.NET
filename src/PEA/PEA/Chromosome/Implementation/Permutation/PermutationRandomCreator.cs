using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PermutationRandomCreator : IChromosomeCreator<PermutationChromosome>
    {
        public int Size { get; }
        public IRandom Random { get; }
        public IList<IConflictDetector> ConflictDetectors { get; }

        public PermutationRandomCreator(int size, IRandom random, IList<IConflictDetector> conflictDetectors)
        {
            Size = size;
            Random = random;
            ConflictDetectors = conflictDetectors;
        }

        public virtual IChromosome Create()
        {
            var genes = CreateIdentityPermutation(Size);
            genes = Shuffle(genes);
            return new PermutationChromosome(genes);
        }

        public int[] CreateIdentityPermutation(int size)
        {
            int[] result = new int[Size];
            for (int i = 0; i < Size; i++)
            {
                result[i] = i;
            }
            return result;
        }

        public int[] Shuffle(int[] shuffled)
        {
            for (int i = shuffled.Length - 1; i > -1; i--)
            {
                int j = Random.GetInt(0, i);
                int tmp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = tmp;
            }
            return shuffled;
        }
    }
}
