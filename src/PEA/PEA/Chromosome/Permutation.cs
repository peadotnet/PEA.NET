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

        public Permutation(IRandom random, IParameterSet parameterSet, IList<IConflictDetector> conflictDetectors)
        {
            var size = parameterSet.GetInt("ProblemSize");

            _creators = new List<IChromosomeCreator>()
            {
                new PermutationRandomCreator(size, random, conflictDetectors)
            };

            _crossovers = new List<ICrossover>()
            {
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
            return new List<PeaSettingsNamedValue>();
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
            engine.Parameters.SetValue(ParameterNames.ConflictReducingProbability, 0.5);
            engine.Parameters.SetValue(ParameterNames.FailedCrossoverRetryCount, 1);
            engine.Parameters.SetValue(ParameterNames.FailedMutationRetryCount, 2);
            return engine;
        }
    }
}
