using System.Collections.Generic;
using Pea.Chromosome.Implementation.SortedSubset;
using Pea.Core;

namespace Pea.Chromosome
{
    public class SortedSubset : IChromosomeFactory<SortedSubsetChromosome>
    {
        private readonly List<ICrossover<SortedSubsetChromosome>> _crossovers;
        private readonly List<IMutation<SortedSubsetChromosome>> _mutations;

        public SortedSubset(IRandom random, IParameterSet parameterSet)
        {
            _crossovers = new List<ICrossover<SortedSubsetChromosome>>()
            {
                new SortedSubsetTwoPointCrossover(random, parameterSet),
                new SortedSubsetOnePointCrossover(random, parameterSet)
            };

            _mutations = new List<IMutation<SortedSubsetChromosome>>()
            {
                new SortedSubsetCreateNewSectionMutation(random, parameterSet),
                new SortedSubsetEliminateSectionMutation(random, parameterSet),
                new SortedSubsetReplaceOneGeneMutation(random, parameterSet),
                new SortedSubsetSwapThreeRangeMutation(random, parameterSet),
                new SortedSubsetSwapTwoRangeMutation(random, parameterSet)
            };
        }

        public IChromosomeFactory<SortedSubsetChromosome> AddCrossovers(IEnumerable<ICrossover<SortedSubsetChromosome>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IChromosomeFactory<SortedSubsetChromosome> AddMutations(IEnumerable<IMutation<SortedSubsetChromosome>> mutations)
        {
            _mutations.AddRange(mutations);
            return this;
        }

        public IEnumerable<ICrossover<SortedSubsetChromosome>> GetCrossovers()
        {
            return _crossovers;
        }

        public IEnumerable<IMutation<SortedSubsetChromosome>> GetMutations()
        {
            return _mutations;
        }
    }
}
