using System.Collections.Generic;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;

namespace Pea.Chromosome
{
    public class Permutation : IChromosomeFactory<PermutationChromosome>
    {
        private readonly List<ICrossover<PermutationChromosome>> _crossovers;
        private readonly List<IMutation<PermutationChromosome>> _mutations;

        public Permutation(IRandom random, IParameterSet parameterSet)
        {
            _crossovers = new List<ICrossover<PermutationChromosome>>()
            {
                new PMXCrossover(random, parameterSet, null)
            };

            _mutations = new List<IMutation<PermutationChromosome>>()
            {
                new RelocateRangeMutation(random, parameterSet, null),
                new InverseRangeMutation(random, parameterSet, null),
                new SwapTwoRangeMutation(random, parameterSet, null)
            };
        }

        public IChromosomeFactory<PermutationChromosome> AddCrossovers(IEnumerable<ICrossover<PermutationChromosome>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IChromosomeFactory<PermutationChromosome> AddMutations(IEnumerable<IMutation<PermutationChromosome>> mutations)
        {
            _mutations.AddRange(mutations);
            return this;
        }

        public IEnumerable<ICrossover> GetCrossovers()
        {
            return _crossovers;
        }

        public IEnumerable<IMutation> GetMutations()
        {
            return _mutations;
        }
    }
}
