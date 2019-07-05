using System;
using System.Linq;
using Pea.Chromosome.Implementation.Permutation;
using Pea.Core;

namespace PEA_TSP_Example
{
    public class TSPEntityCreator : IEntityCreator
    {
        private int Count { get; set; } = 52;

        public void Init(IEvaluationInitData initData)
        {
            Count = ((TSPInitData) initData).TSPPoints.Count;
        }

        public IEntity CreateEntity()
        {
            var entity = new TSPEntity();
            var genes = ShuffleRange(0, Count);
            entity.Chromosomes.Add("TSP", new PermutationChromosome(genes));
            return entity;
        }

        private int[] ShuffleRange(int start, int count)
        {
            int[] shuffled = Enumerable.Range(start, count).ToArray();
            Random rng = new Random();
            for (int i = shuffled.Length - 1; i > -1; i--)
            {
                int j = rng.Next(i);
                int tmp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = tmp;
            }
            return shuffled;
        }
    }
}
