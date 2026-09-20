using BenchmarkDotNet.Attributes;
using Pea.Core;
using Pea.Core.Entity;
using System;
using System.Collections.Generic;

namespace PEA.Benchmarks.CoreBenchmarks
{
	[MemoryDiagnoser]
	[JsonExporterAttribute.Full]
	public class ListHashSetComparisonBenchmark
	{
		public class TestEntity : EntityBase
		{
			public double SomeValue { get; }

			public TestEntity(double someValue) : base(1)
			{
				SomeValue = someValue;
			}
		}


		[Params(20, 22, 25)]
		public int Count { get; set; }

		List<EntityBase> Entities = new List<EntityBase>();

		public Random random = new Random(DateTime.Now.Millisecond);


		[GlobalSetup]
		public void Setup()
		{
			for (int i = 0; i < 1000; i++)
			{
				var someValue = random.NextDouble();
				Entities.Add(new TestEntity(someValue));
			}
		}

		[Benchmark]
		public List<EntityBase> SelectWithHashSet()
		{
			var result = new HashSet<EntityBase>();
			for (int i = 0; i < Count; i++)
			{
				EntityBase entity = SelectOne();
				while (!result.Add(entity))
				{
					entity = SelectOne();
				}
			}

			return new List<EntityBase>(result);
		}

		[Benchmark]
		public List<EntityBase> SelectWithList()
		{
			var result = new List<EntityBase>(Count);
			for (int i=0; i< Count; i++)
			{
				EntityBase entity = SelectOne();
				while (result.Contains(entity))
				{
					entity = SelectOne();
				}
				result.Add(entity);
			}

			return result;
		}

		private EntityBase SelectOne()
		{
			var index = random.Next(0, 1000);
			var entity = Entities[index];
			return entity;
		}
	}
}
