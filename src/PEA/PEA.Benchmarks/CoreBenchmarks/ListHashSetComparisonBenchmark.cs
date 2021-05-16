using BenchmarkDotNet.Attributes;
using Pea.Core;
using Pea.Core.Entity;
using System;
using System.Collections.Generic;

namespace PEA.Benchmarks.CoreBenchmarks
{
	public class ListHashSetComparisonBenchmark
	{
		public class TestEntity : EntityBase
		{
			public double SomeValue { get; }

			public TestEntity(double someValue)
			{
				SomeValue = someValue;
			}
		}


		[Params(20, 22, 25)]
		public int Count { get; set; }

		List<IEntity> Entities = new List<IEntity>();

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
		public List<IEntity> SelectWithHashSet()
		{
			var result = new HashSet<IEntity>();
			for (int i = 0; i < Count; i++)
			{
				IEntity entity = SelectOne();
				while (!result.Add(entity))
				{
					entity = SelectOne();
				}
			}

			return new List<IEntity>(result);
		}

		[Benchmark]
		public List<IEntity> SelectWithList()
		{
			var result = new List<IEntity>(Count);
			for (int i=0; i< Count; i++)
			{
				IEntity entity = SelectOne();
				while (result.Contains(entity))
				{
					entity = SelectOne();
				}
				result.Add(entity);
			}

			return result;
		}

		private IEntity SelectOne()
		{
			var index = random.Next(0, 1000);
			var entity = Entities[index];
			return entity;
		}
	}
}
