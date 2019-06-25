using System.Collections.Generic;
using FluentAssertions;
using Pea.Core;
using Pea.Fitness.Implementation.MultiObjective;
using Pea.Island;
using Xunit;

namespace Pea.Tests.EngineTests
{
    public class PeaSettingTests
    {
        public class testEvaluator : IFitnessEvaluator
        {
            public IList<IEntity> AssessFitness(IList<IEntity> entityList)
            {
                return new List<IEntity>();
            }
        }

        public class testDecoder : IPhenotypeDecoder
        {
            public void Init(IPhenotypeDecoderInitData initData)
            {
            }

            public IPhenotype Decode(IGenotype genotype)
            {
                return null;
            }
        }

        [Fact]
        public void PeaSettings_CreateEngine_ShouldGetProperties()
        {
            var system = PeaSystem.Create()
                .WithAlgorithm<Algorithm.SteadyState>()
                .AddChromosome<Chromosome.SortedSubset>("TransitServices")
                .WithPhenotypeDecoder<testDecoder>()
                .WithFitness<Fitness.ParetoMultiobjective>()
                .WithFitnessEvaluator<testEvaluator>()
                .AddSelection<Selection.TournamentSelection>()
                .AddReinsertion<Reinsertion.ReplaceParentsReinsertion>();

            system.Settings.Random = typeof(SystemRandom);

            var islandEngine = IslandEngineFactory.Create(system.Settings);

            islandEngine.FitnessComparer.Should().BeOfType<NonDominatedParetoComparer>();

        }
    }
}
