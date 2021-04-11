using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.BitVector
{
	public class BitVectorZeroCreator : IChromosomeCreator<BitVectorChromosome>
	{
		public int Size { get; }
		public IRandom Random { get; }
		public IList<IConflictDetector> ConflictDetectors { get; }

		public BitVectorZeroCreator(int size, IRandom random, IList<IConflictDetector> conflictDetectors)
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

		private bool[] CreateRandomGenes(int Size)
		{
			var genes = new bool[Size];
			return genes;
		}
	}
}
