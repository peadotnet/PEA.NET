using Pea.Chromosome.Implementation.BitVector;
using Pea.Configuration.Implementation;
using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome
{
	public class BitVector : IChromosomeFactory<BitVectorChromosome>
	{
		private readonly List<IChromosomeCreator> _creators;
		private readonly List<ICrossover> _crossovers;
		private readonly List<IMutation> _mutations;

		public BitVector(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
		{
			var size = parameterSet.GetInt("ProblemSize");

			_creators = new List<IChromosomeCreator>()
			{
				new BitVectorRandomCreator(size, random, conflictDetectors)
			};

			parameterSet.SetValueRange(GetParameters());

			_crossovers = new List<ICrossover>()
			{
                new DoNothingCrossover(random, parameterSet, conflictDetectors),
				new TwoPointCrossover(random, parameterSet, conflictDetectors)
			};

			_mutations = new List<IMutation>()
			{
				new DoNothingMutation(random, parameterSet, conflictDetectors),
				new PointMutation(random, parameterSet, conflictDetectors)
			};
		}

		public IChromosomeFactory<BitVectorChromosome> AddCreators(IEnumerable<IChromosomeCreator<BitVectorChromosome>> creators)
		{
			_creators.AddRange(creators);
			return this;
		}

		public IChromosomeFactory<BitVectorChromosome> AddCrossovers(IEnumerable<ICrossover<BitVectorChromosome>> crossovers)
		{
			_crossovers.AddRange(crossovers);
			return this;
		}

		public IChromosomeFactory<BitVectorChromosome> AddMutations(IEnumerable<IMutation<BitVectorChromosome>> mutations)
		{
			_mutations.AddRange(mutations);
			return this;
		}

		public IEngine Apply(IEngine engine)
		{
			engine.Parameters.SetValueRange(GetParameters());
			return engine;
		}

		public IList<IChromosomeCreator> GetCreators()
		{
			return _creators;
		}

		public IList<ICrossover> GetCrossovers()
		{
			return _crossovers;
		}

		public IList<IMutation> GetMutations()
		{
			return _mutations;
		}

		public IEnumerable<PeaSettingsNamedValue> GetParameters()
		{
			return new List<PeaSettingsNamedValue>()
			{
				//new PeaSettingsNamedValue(ParameterNames.ConflictReducingProbability, 0.5),
				//new PeaSettingsNamedValue(ParameterNames.FailedCrossoverRetryCount, 1),
				//new PeaSettingsNamedValue(ParameterNames.FailedMutationRetryCount, 2),
				//new PeaSettingsNamedValue(ParameterNames.MutationProbability, 0.5),
				//new PeaSettingsNamedValue(ParameterNames.MutationIntensity, 300),
				//new PeaSettingsNamedValue(ParameterNames.BlockSize, 2)
			};

		}

	}
}
