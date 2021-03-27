using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class BitVectorRandomCreator : IChromosomeCreator<BitVectorChromosome>
	{
		public int Size { get; }
		public IRandom Random { get; }
		public IList<IConflictDetector> ConflictDetectors { get; }

		public BitVectorRandomCreator(int size, IRandom random, IList<IConflictDetector> conflictDetectors)
		{
			Size = size;
			Random = random;
			ConflictDetectors = conflictDetectors;
		}

		public IChromosome Create()
		{
			var genes = CreateRandomGenes(Size);
			return new BitVectorChromosome(genes);
		}

		private bool[] CreateRandomGenes(int size)
		{
			var genes = new bool[size];
			for(int i=0; i<size; i++)
			{
				var gene = Random.GetInt(0, 2);
				genes[i] = (gene == 0);
			}
			return genes;
		}
	}
}
