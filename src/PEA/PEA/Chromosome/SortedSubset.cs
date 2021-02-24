using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Chromosome
{
    public class SortedSubset : ChromosomeFactory, IChromosomeFactory<SortedSubsetChromosome>
    {
        private IList<INeighborhoodConflictDetector> ConflictDetectors { get; }

        private readonly List<IChromosomeCreator> _creators;
        private readonly List<ICrossover> _crossovers;
        private readonly List<IMutation> _mutations;

        public SortedSubset(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors = null)
        {
            var size = parameterSet.GetInt("ProblemSize");

            parameterSet.SetValueRange(GetParameters());

            ConflictDetectors = conflictDetectors;

            _creators = new List<IChromosomeCreator>()
            {
                new SortedSubsetLeftToRightCreator(size, random, conflictDetectors)
            };

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

        public IEnumerable<PeaSettingsNamedValue> GetParameters()
        {
            return new List<PeaSettingsNamedValue>()
            {
                new PeaSettingsNamedValue(ParameterNames.ConflictReducingProbability, 0.5),
                new PeaSettingsNamedValue(ParameterNames.FailedCrossoverRetryCount, 10),
                new PeaSettingsNamedValue(ParameterNames.FailedMutationRetryCount, 20),
                new PeaSettingsNamedValue(ParameterNames.MutationProbability, 0.5),
                new PeaSettingsNamedValue(ParameterNames.MutationIntensity, 0.1)
            };
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
            engine.Parameters.SetValueRange(GetParameters());
            return engine;
        }
    }
}
