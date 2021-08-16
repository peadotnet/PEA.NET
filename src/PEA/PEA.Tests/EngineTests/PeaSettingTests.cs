﻿using System.Collections.Generic;
using Pea.Core;
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

            public override IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
            {
                throw new System.NotImplementedException();
            }

            public override IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities)
            {
                throw new System.NotImplementedException();
            }

            public IEntity AssessFitness(IEntity entity)
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
