using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Pea.Core;
using Pea.Core.Entity;
using Xunit;

namespace Pea.Tests.CoreTests
{
    public class EntityCrossoverTests
    {
        private const string GeneKey = "genes";

        [Fact]
        public void GivenCrossoverReturningFirstParentInstance_WhenCross_ThenThrowsInvalidOperationException()
        {
            var crossover = new StubCrossover((parent0, parent1) => new List<IChromosome> { parent0, new StubChromosome(9) });

            Action cross = () => CreateSut(crossover).Cross(TwoParents(), 2);

            cross.Should().Throw<InvalidOperationException>().WithMessage($"*{nameof(StubCrossover)}*");
        }

        [Fact]
        public void GivenCrossoverReturningSecondParentInstance_WhenCross_ThenThrowsInvalidOperationException()
        {
            var crossover = new StubCrossover((parent0, parent1) => new List<IChromosome> { new StubChromosome(9), parent1 });

            Action cross = () => CreateSut(crossover).Cross(TwoParents(), 2);

            cross.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void GivenCrossoverReturningNewInstances_WhenCross_ThenOffspringHoldTheReturnedChromosomes()
        {
            var child0 = new StubChromosome(10);
            var child1 = new StubChromosome(11);
            var crossover = new StubCrossover((parent0, parent1) => new List<IChromosome> { child0, child1 });

            var offspring = CreateSut(crossover).Cross(TwoParents(), 2);

            offspring[0].Chromosomes[GeneKey].Should().BeSameAs(child0);
            offspring[1].Chromosomes[GeneKey].Should().BeSameAs(child1);
        }

        [Fact]
        public void GivenNewChildren_WhenAssertThatChromosomesAreNewInstances_ThenDoesNotThrow()
        {
            var parent0 = new StubChromosome(1);
            var parent1 = new StubChromosome(2);
            var children = new List<IChromosome> { parent0.DeepClone(), parent1.DeepClone() };

            Action check = () => EntityCrossover.AssertThatChromosomesAreNewInstances(children, parent0, parent1, new StubCrossover(null));

            check.Should().NotThrow();
        }

        private static IEntityCrossover CreateSut(ICrossover crossover)
        {
            var factory = Substitute.For<IChromosomeFactory>();
            factory.GetCrossovers().Returns(new List<ICrossover> { crossover });

            var factories = new Dictionary<string, IChromosomeFactory> { { GeneKey, factory } };
            return new EntityCrossover(factories, new FastRandom(1));
        }

        private static IEntityList TwoParents()
        {
            var list = new EntityList(2);
            list.Add(CreateEntity(new StubChromosome(1)));
            list.Add(CreateEntity(new StubChromosome(2)));
            return list;
        }

        private static EntityBase CreateEntity(IChromosome chromosome)
        {
            var entity = new TestEntity();
            entity.Chromosomes.Add(GeneKey, chromosome);
            return entity;
        }

        private sealed class TestEntity : EntityBase
        {
            public TestEntity() : base(1) { }
        }

        private sealed class StubChromosome : IChromosome
        {
            public StubChromosome(int id) => Id = id;

            public int Id { get; }

            public IChromosome DeepClone() => new StubChromosome(Id);
        }

        private sealed class StubCrossover : ICrossover
        {
            private readonly Func<IChromosome, IChromosome, IList<IChromosome>> _cross;

            public StubCrossover(Func<IChromosome, IChromosome, IList<IChromosome>> cross) => _cross = cross;

            public IList<IChromosome> Cross(IChromosome parent0, IChromosome parent1) => _cross(parent0, parent1);
        }
    }
}
