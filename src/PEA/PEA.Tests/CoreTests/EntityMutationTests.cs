using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Pea.Core;
using Pea.Core.Entity;
using Xunit;

namespace Pea.Tests.CoreTests
{
    public class EntityMutationTests
    {
        private const string GeneKey = "genes";

        [Fact]
        public void GivenEntity_WhenMutate_ThenTheGivenInstanceReceivesTheMutatedChromosome()
        {
            var replacement = new StubChromosome(99);
            var mutation = new StubMutation(_ => replacement);
            var entity = CreateEntity(new StubChromosome(1));

            CreateSut(mutation).Mutate(ListOf(entity));

            entity.Chromosomes[GeneKey].Should().BeSameAs(replacement);
        }

        [Fact]
        public void GivenEntity_WhenMutate_ThenMutationReceivesTheChromosomeHeldByTheEntity()
        {
            var original = new StubChromosome(1);
            var mutation = new StubMutation(chromosome => chromosome);
            var entity = CreateEntity(original);

            CreateSut(mutation).Mutate(ListOf(entity));

            mutation.Received.Should().ContainSingle().Which.Should().BeSameAs(original);
        }

        [Fact]
        public void GivenSeveralEntities_WhenMutate_ThenEachEntityIsMutatedOnce()
        {
            var mutation = new StubMutation(chromosome => chromosome);
            var first = CreateEntity(new StubChromosome(1));
            var second = CreateEntity(new StubChromosome(2));
            var third = CreateEntity(new StubChromosome(3));

            CreateSut(mutation).Mutate(ListOf(first, second, third));

            mutation.Received.Should().HaveCount(3);
        }

        [Fact]
        public void GivenAlreadyMutatedEntity_WhenMutatedAgain_ThenShouldNotThrow()
        {
            var mutation = new StubMutation(chromosome => chromosome);
            var sut = CreateSut(mutation);
            var entity = CreateEntity(new StubChromosome(1));
            sut.Mutate(ListOf(entity));

            Action mutateAgain = () => sut.Mutate(ListOf(entity));

            mutateAgain.Should().NotThrow();
        }

        [Fact]
        public void GivenEntity_WhenMutate_ThenLastMutationsRecordsTheOperatorName()
        {
            var mutation = new StubMutation(chromosome => chromosome);
            var entity = CreateEntity(new StubChromosome(1));

            CreateSut(mutation).Mutate(ListOf(entity));

            entity.LastMutations[GeneKey].Should().Be(nameof(StubMutation));
        }

        [Fact]
        public void GivenEntityMutatedTwice_WhenMutate_ThenLastMutationsHoldsTheMostRecentOperator()
        {
            var entity = CreateEntity(new StubChromosome(1));
            CreateSut(new StubMutation(chromosome => chromosome)).Mutate(ListOf(entity));

            CreateSut(new OtherStubMutation()).Mutate(ListOf(entity));

            entity.LastMutations[GeneKey].Should().Be(nameof(OtherStubMutation));
        }

        [Fact]
        public void GivenMutationFailingOnce_WhenMutate_ThenShouldRetryUntilItSucceeds()
        {
            var replacement = new StubChromosome(99);
            var attempts = 0;
            var mutation = new StubMutation(_ => attempts++ == 0 ? null : replacement);
            var entity = CreateEntity(new StubChromosome(1));

            CreateSut(mutation).Mutate(ListOf(entity));

            attempts.Should().Be(2);
            entity.Chromosomes[GeneKey].Should().BeSameAs(replacement);
        }

        private static IEntityMutation CreateSut(IMutation mutation)
        {
            var factory = Substitute.For<IChromosomeFactory>();
            factory.GetMutations().Returns(new List<IMutation> { mutation });

            var factories = new Dictionary<string, IChromosomeFactory> { { GeneKey, factory } };
            return new EntityMutation(factories, new FastRandom(1));
        }

        private static EntityBase CreateEntity(IChromosome chromosome)
        {
            var entity = new EntityBase(1);
            entity.Chromosomes.Add(GeneKey, chromosome);
            return entity;
        }

        private static IEntityList ListOf(params EntityBase[] entities)
        {
            var list = new EntityList(entities.Length);
            foreach (var entity in entities) list.Add(entity);
            return list;
        }

        private sealed class StubChromosome : IChromosome
        {
            public StubChromosome(int id) => Id = id;

            public int Id { get; }

            public IChromosome DeepClone() => new StubChromosome(Id);
        }

        private sealed class OtherStubMutation : IMutation
        {
            public IChromosome Mutate(IChromosome chromosome) => chromosome;
        }

        private sealed class StubMutation : IMutation
        {
            private readonly Func<IChromosome, IChromosome> _mutate;

            public StubMutation(Func<IChromosome, IChromosome> mutate) => _mutate = mutate;

            public List<IChromosome> Received { get; } = new List<IChromosome>();

            public IChromosome Mutate(IChromosome chromosome)
            {
                Received.Add(chromosome);
                return _mutate(chromosome);
            }
        }
    }
}
