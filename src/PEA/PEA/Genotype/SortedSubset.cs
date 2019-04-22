using System.Collections.Generic;
using Pea.Core;
using Pea.Genotype.Implementation.SortedSubset;

namespace Pea.Genotype
{
    public class SortedSubset : IGenotypeFactory<SortedSubsetGenotype>
    {
        List<IGenotypeCreator<SortedSubsetGenotype>> _creators;
        List<ICrossover<SortedSubsetGenotype>> _crossovers;
        List<IMutation<SortedSubsetGenotype>> _mutations;

        public SortedSubset()
        {
            _creators = new List<IGenotypeCreator<SortedSubsetGenotype>>();

            _crossovers = new List<ICrossover<SortedSubsetGenotype>>()
            {
                new SortedSubsetTwoPointCrossover(),
                new SortedSubsetOnePointCrossover()
            };

            _mutations = new List<IMutation<SortedSubsetGenotype>>()
            {
                new SortedSubsetCreateNewChromosomeMutation(),
                new SortedSubsetEliminateChromosomeMutation(),
                new SortedSubsetReplaceGeneMutation(),
                new SortedSubsetReplaceRangeMutation(),
                new SortedSubsetSwapThreeRangeMutation(),
                new SortedSubsetSwapTwoRangeMutation()
            };
        }

        public IGenotypeFactory<SortedSubsetGenotype> AddCreators(IEnumerable<IGenotypeCreator<SortedSubsetGenotype>> creators)
        {
            _creators.AddRange(creators);
            return this;
        }

        public IGenotypeFactory<SortedSubsetGenotype> AddCrossovers(IEnumerable<ICrossover<SortedSubsetGenotype>> crossovers)
        {
            _crossovers.AddRange(crossovers);
            return this;
        }

        public IGenotypeFactory<SortedSubsetGenotype> AddMutations(IEnumerable<IMutation<SortedSubsetGenotype>> mutations)
        {
            _mutations.AddRange(mutations);
            return this;
        }

        public IEnumerable<IGenotypeCreator<SortedSubsetGenotype>> GetCreators()
        {
            return _creators;
        }

        public IEnumerable<ICrossover<SortedSubsetGenotype>> GetCrossovers()
        {
            return _crossovers;
        }

        public IEnumerable<IMutation<SortedSubsetGenotype>> GetMutations()
        {
            return _mutations;
        }
    }
}
