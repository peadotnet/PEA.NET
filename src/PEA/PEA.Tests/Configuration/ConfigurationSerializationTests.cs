using Akka.Actor;
using Akka.Serialization;
using FluentAssertions;
using Pea.Algorithm;
using Pea.Configuration;
using Pea.Configuration.Implementation;
using Pea.Core;
using Pea.Population.Replacement;
using Pea.Selection;
using Xunit;
using PeaSettings = Pea.Configuration.Implementation.PeaSettings;

namespace Pea.Tests.Configuration
{
    public class ConfigurationSerializationTests
    {
        [Fact]
        public void SubProblemBuilder_Serialize_ShouldReturnSame()
        {
            var subProblem = new SubProblemBuilder()
                .WithEncoding<Chromosome.Permutation>("TSP")
                    .WithAlgorithm<SteadyState>()
                        .Selections.Add<TournamentSelection>()
                        .ReInsertions.Add<ReplaceWorstParentWithBestChildrenReinsertion>()
                    .Operators.Clear()
                    .Operators.Add<Chromosome.Implementation.Permutation.ShuffleRangeMutation>()
                    .AddConflictDetector<AllRightConflictDetector>()
                .SetParameter("SomeParameter", 3.141592654)
                .Build();

            var result = SerializeAndDeserialize<SubProblem>(subProblem);

            result.Should().BeEquivalentTo(subProblem);
        }

        [Fact]
        public void PeaSettingsBuilder_Serialize_ShouldReturnSame()
        {
            var settings = new PeaSettingsBuilder();

            settings.AddSubProblem().WithEncoding<Chromosome.Permutation>("TSP");
            settings.AddSubProblem().WithEncoding<Chromosome.SortedSubset>("VSP");
            settings.StopWhen().TimeoutElapsed(10000);

            var original = settings.Build();
            var result = SerializeAndDeserialize<PeaSettings>(original);

            result.Should().BeEquivalentTo(original);
        }
        
        private static T SerializeAndDeserialize<T>(T config)
        {
            ActorSystem system = ActorSystem.Create("example");
            Serialization serialization = system.Serialization;
            Serializer serializer = serialization.FindSerializerFor(config);
            byte[] bytes = serializer.ToBinary(config);
            string json = System.Text.Encoding.Default.GetString(bytes);
            var result = (T)serializer.FromBinary(bytes, typeof(T));
            return result;
        }
    }
}
