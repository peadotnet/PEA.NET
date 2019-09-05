using Akka.Actor;
using Akka.Serialization;
using FluentAssertions;
using Pea.Configuration;
using Pea.Configuration.Implementation;
using Xunit;

namespace Pea.Tests.Configuration
{
    public class ConfigurationSerializationTests
    {
        [Fact]
        public void SubProblemBuilder_Serialize_ShouldReturnSame()
        {
            var subProblem = new SubProblemBuilder()
                .Encoding<Chromosome.Permutation>("TSP")
                    .Operators.Clear()
                    .Operators.Add(typeof(Chromosome.Implementation.Permutation.ShuffleRangeMutation))
                .Build();

            var result = SerializeAndDeserialize<SubProblem>(subProblem);

            result.Should().BeEquivalentTo(subProblem);
        }

        private static T SerializeAndDeserialize<T>(T config)
        {
            ActorSystem system = ActorSystem.Create("example");
            Serialization serialization = system.Serialization;
            Serializer serializer = serialization.FindSerializerFor(config);
            byte[] bytes = serializer.ToBinary(config);
            var result = (T)serializer.FromBinary(bytes, typeof(T));
            return result;
        }
    }
}
