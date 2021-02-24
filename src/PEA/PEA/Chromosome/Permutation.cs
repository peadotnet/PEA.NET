using System.Collections.Generic;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Chromosome
{
    public class Permutation : IChromosomeFactory<PermutationChromosome>
    {
        private readonly List<IChromosomeCreator> _creators;
        private readonly List<ICrossover> _crossovers;
        private readonly List<IMutation> _mutations;

        public Permutation(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
        {
            var size = parameterSet.GetInt("ProblemSize");

            _creators = new List<IChromosomeCreator>()
            {
                new PermutationRandomCreator(size, random, conflictDetectors)
            };

            _crossovers = new List<ICrossover>()
            {
                new DoNothingCrossover(random, parameterSet, conflictDetectors),
                new Order1Crossover(random, parameterSet, conflictDetectors),
                new PMXCrossover(random, parameterSet, conflictDetectors)
            };

            _mutations = new List<IMutation>()
            {
                new RelocateRangeMutation(random, parameterSet, conflictDetectors),
                new InverseRangeMutation(random, parameterSet, conflictDetectors),
                new SwapTwoRangeMutation(random, parameterSet, conflictDetectors),
                new ShuffleRangeMutation(random, parameterSet, conflictDetectors),
                new RelocateOneMutation(random, parameterSet, conflictDetectors)
            };
        }

        public IChromosomeFactory<PermutationChromosome> AddCrossovers(IEnumerable<ICrossover<PermutationChromosome>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IChromosomeFactory<PermutationChromosome> AddCreators(IEnumerable<IChromosomeCreator<PermutationChromosome>> creators)
        {
            _creators.AddRange(creators);
            return this;
        }

        public IChromosomeFactory<PermutationChromosome> AddMutations(IEnumerable<IMutation<PermutationChromosome>> mutations)
        {
            _mutations.AddRange(mutations);
            return this;
        }

        public IEnumerable<PeaSettingsNamedValue> GetParameters()
        {
            return new List<PeaSettingsNamedValue>()
            {
                new PeaSettingsNamedValue(ParameterNames.ConflictReducingProbability, 0.5),
                new PeaSettingsNamedValue(ParameterNames.FailedCrossoverRetryCount, 1),
                new PeaSettingsNamedValue(ParameterNames.FailedMutationRetryCount, 2),
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

        public IEngine Apply(IEngine engine)
        {
            engine.Parameters.SetValueRange(GetParameters());
            return engine;
        }
    }
}
