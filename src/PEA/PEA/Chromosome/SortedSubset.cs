using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;

namespace Pea.Chromosome
{
    public class SortedSubset : ChromosomeFactory, IChromosomeFactory<SortedSubsetChromosome>
    {
        private IList<IConflictDetector> ConflictDetectors { get; }

        private readonly List<IChromosomeCreator> _creators;
        private readonly List<ICrossover> _crossovers;
        private readonly List<IMutation> _mutations;

        public SortedSubset(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors = null)
        {
            ConflictDetectors = conflictDetectors;

            _crossovers = new List<ICrossover>()
            {
                new TwoPointCrossover(random, parameterSet, conflictDetectors),
                new OnePointCrossover(random, parameterSet, conflictDetectors)
            };

            _mutations = new List<IMutation>()
            {
                //new CreateNewSectionMutation(random, parameterSet, conflictDetector),
                new EliminateSectionMutation(random, parameterSet, conflictDetectors),
                new ReplaceOneGeneMutation(random, parameterSet, conflictDetectors),
                new SwapThreeRangeMutation(random, parameterSet, conflictDetectors),
                new SwapTwoRangeMutation(random, parameterSet, conflictDetectors)
            };
        }

        public IChromosomeFactory<SortedSubsetChromosome> AddCrossovers(IEnumerable<ICrossover<SortedSubsetChromosome>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IChromosomeFactory<SortedSubsetChromosome> AddCreators(IEnumerable<IChromosomeCreator<SortedSubsetChromosome>> creators)
        {
            throw new System.NotImplementedException();
        }

        public IChromosomeFactory<SortedSubsetChromosome> AddMutations(IEnumerable<IMutation<SortedSubsetChromosome>> mutations)
        {
            _mutations.AddRange(mutations);
            return this;
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

        public override IEngine Apply(IEngine engine)
        {
            engine.Parameters.SetValue(ParameterNames.ConflictReducingProbability, 0.5);
            engine.Parameters.SetValue(ParameterNames.FailedCrossoverRetryCount, 3);
            engine.Parameters.SetValue(ParameterNames.FailedMutationRetryCount, 3);
            return engine;
        }
    }
}
