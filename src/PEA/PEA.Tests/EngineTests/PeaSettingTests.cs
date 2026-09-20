using Pea.Core;
using Pea.Core.Entity;
using System.Collections.Generic;
using Xunit;

namespace Pea.Tests.EngineTests
{
    public class PeaSettingTests
    {
        public class TestEvaluation : EvaluationBase
        {
            public TestEvaluation(ParameterSet parameterSet) : base(parameterSet) { }

            public override void Init(IEvaluationInitData initData)
            {
                throw new System.NotImplementedException();
            }

            public override EntityBase Decode(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities)
            {
                throw new System.NotImplementedException();
            }

            public override List<EntityBase> Combine(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities)
            {
                throw new System.NotImplementedException();
            }

            public EntityBase AssessFitness(EntityBase entity)
            {
                throw new System.NotImplementedException();
            }
		}

        [Fact]
        public void PeaSettings_CreateEngine_ShouldGetProperties()
        {
            //var system = PeaSystem.Create()
            //    .WithAlgorithm<Algorithm.SteadyState>()
            //    .AddChromosome<Chromosome.SortedSubset>("TransitServices")
            //    .WithFitness<Fitness.ParetoMultiobjective>()
            //    .AddSelection<Selection.TournamentSelection>()
            //    .AddReinsertion<Reinsertion.ReplaceParentsReinsertion>()
            //    .WithEvaluation<testEvaluation>();

            //system.Settings.Random = typeof(SystemRandom);

            //var islandEngine = IslandEngineFactory.Create(system.Settings);

            //islandEngine.FitnessComparer.Should().BeOfType<NonDominatedParetoComparer>();

        }
    }
}
