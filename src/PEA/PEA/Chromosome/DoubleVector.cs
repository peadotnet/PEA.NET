using Pea.Chromosome.Implementation.DoubleVector;
using Pea.Configuration.Implementation;
using Pea.Core;
using System.Collections.Generic;

namespace Pea.Chromosome
{
    public class DoubleVector : IChromosomeFactory<DoubleVectorChromosome>
    {
        private readonly List<IChromosomeCreator> _creators;
        private readonly List<ICrossover> _crossovers;
        private readonly List<IMutation> _mutations;

        public DoubleVector(IRandom random, IParameterSet parameterSet, IList<INeighborhoodConflictDetector> conflictDetectors)
        {
            _creators = new List<IChromosomeCreator>()
            {
                //new PermutationRandomCreator(size, random, conflictDetectors)
            };

            _crossovers = new List<ICrossover>()
            {
                new OnePointCrossover(random, parameterSet, conflictDetectors),
                new TwoPointCrossover(random, parameterSet, conflictDetectors),
                new UniformCrossover(random, parameterSet, conflictDetectors),
                new InterpolationCrossover(random, parameterSet, conflictDetectors),
                new UniformInterpolationCrossover(random, parameterSet, conflictDetectors)
            };

            _mutations = new List<IMutation>()
            {
                new DoNothingMutation(random, parameterSet, conflictDetectors),
                new GaussianMutation(random, parameterSet, conflictDetectors)
            };
        }

        public IChromosomeFactory<DoubleVectorChromosome> AddCreators(IEnumerable<IChromosomeCreator<DoubleVectorChromosome>> creators)
        {
            _creators.AddRange(creators);
            return this;
        }

        public IChromosomeFactory<DoubleVectorChromosome> AddCrossovers(IEnumerable<ICrossover<DoubleVectorChromosome>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IChromosomeFactory<DoubleVectorChromosome> AddMutations(IEnumerable<IMutation<DoubleVectorChromosome>> mutations)
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
                new PeaSettingsNamedValue(ParameterNames.ConflictReducingProbability, 0.5),
                new PeaSettingsNamedValue(ParameterNames.FailedCrossoverRetryCount, 1),
                new PeaSettingsNamedValue(ParameterNames.FailedMutationRetryCount, 2),
                new PeaSettingsNamedValue(ParameterNames.MutationProbability, 0.5),
                new PeaSettingsNamedValue(ParameterNames.MutationIntensity, 0.1)
            };

        }
    }
}
