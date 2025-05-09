using FluentAssertions;
using Pea.Configuration;
using Pea.Core;
using System.Diagnostics;
using Xunit;

namespace Pea.Tests.AlgorithmTests
{
    public class AlgorithmBaseTests
    {
        [Fact]
        public async Task SlowCreator_InitPopulation_ShouldReturnEmpty()
        {
            var optimizer = Optimizer.Create();
            optimizer.Settings.AddSubProblem("POS", new SlowScenario());
            optimizer.Settings.WithEntityType<TestEntity>().WithEvaluation<TestEvaluation>();
            optimizer.Settings.StopWhen().TimeoutElapsed(20000);                
            optimizer.SetParameter(ParameterNames.PopulationInitTimeout, 10000);
            optimizer.SetParameter(Core.Island.ParameterNames.IslandsCount, 1);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = await optimizer.Run(new TestInitData());
            stopWatch.Stop();

            stopWatch.ElapsedMilliseconds.Should().BeLessThan(11000);
            result.BestSolutions.Should().HaveCount(0);

        }

        public class SlowScenario : IScenario
        {
            public SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder)
            {
                return subProblemBuilder
                    .WithEncoding<Chromosome.DoubleVector>(key)
                    .WithAlgorithm<Algorithm.SteadyState>()
                    .AddEntityCreator<SlowCreator>()
                    .SetParameter(Algorithm.ParameterNames.FitnessLength, 1)
                    .SetParameter(Pea.Algorithm.ParameterNames.PopulationSize, 100);
            }
        }

        public class SlowCreator : IEntityCreator
        {
            public SlowCreator(IRandom random)
            {
            }

            public IEntity CreateEntity()
            {
                Thread.Sleep(1000);
                return null;
            }

            public void Init(IEvaluationInitData initData)
            {
            }
        }
    }
}
