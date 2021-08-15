using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome.Implementation.StructVector
{
	public class StructChromosome<TS>: IChromosome, IDeepCloneable<StructChromosome<TS>> where TS: struct
	{
		public TS[] Genes { get; set; }

		public StructChromosome(ICollection<TS> genes)
		{
			Genes = new TS[genes.Count];
			genes.CopyTo(Genes, 0);
		}

		public StructChromosome<TS> DeepClone()
		{
			return new StructChromosome<TS>(Genes);
		}

		IChromosome IDeepCloneable<IChromosome>.DeepClone()
		{
			return DeepClone();
		}
	}
}
