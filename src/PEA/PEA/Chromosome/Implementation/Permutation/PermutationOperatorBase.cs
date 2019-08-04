using Pea.Core;
using System;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class PermutationOperatorBase
    {
        public IConflictDetector ConflictDetector { get; set; }

        protected readonly IRandom Random;
        protected readonly IParameterSet ParameterSet;

        protected PermutationOperatorBase(IRandom random, IParameterSet parameterSet, IConflictDetector conflictDetector = null)
        {
            Random = random;
            ParameterSet = parameterSet;
            ConflictDetector = conflictDetector;
        }

        public GeneRange GetSourceRange(PermutationChromosome chromosome)
        {
            return ConflictShouldBeEliminated(chromosome)
                ? GetConflictedRange(chromosome)
                : GetRandomRange(chromosome);
        }

        public GeneRange GetRandomRange(PermutationChromosome chromosome)
        {
            var sourcePosition = Random.GetInt(0, chromosome.Genes.Length);
            var sourceLength = Random.GetInt(0, chromosome.Genes.Length - sourcePosition); 
            return new GeneRange(sourcePosition, sourceLength);
        }

        public GeneRange GetConflictedRange(PermutationChromosome chromosome)
        {
            var conflicted = Random.GetInt(0, chromosome.ConflictList.Count);
            var sourcePosition = chromosome.ConflictList[conflicted];
            return sourcePosition;
        }

        public bool ConflictShouldBeEliminated(PermutationChromosome chromosome)
        {
            if (chromosome.ConflictList.Count == 0) return false;

            var reducingConflictPossibility = ParameterSet.GetValue(ParameterNames.ConflictReducingProbability);
            var rnd = Random.GetDouble(0, 1);
            return (rnd < reducingConflictPossibility);
        }

        public int GetRandomPositionWithTabuRange(PermutationChromosome chromosome, GeneRange tabuRange)
        {
            int position = 0;
            bool getFromEnd = false;
            if (tabuRange.Position == 0)
            {
                getFromEnd = true;
            }
            else if (tabuRange.Position + tabuRange.Length < chromosome.Genes.Length)
            {
                var decision = Random.GetDouble(0, 1);
                if (decision > 0.5) getFromEnd = true;
            }

            if(getFromEnd)
            {
                position = Random.GetInt(tabuRange.Position + tabuRange.Length, chromosome.Genes.Length);
            }
            else
            {
                position = Random.GetInt(0, tabuRange.Position);
            }
            return position;
        }

        public int[] SwapTwoRange(int[] genes, GeneRange range1, GeneRange range2)
        {
            if (range2.Position < range1.Position)
            {
                var r = range2;
                range2 = range1;
                range1 = r;
            }

            if (range2.Position < range1.Position + range1.Length)
            {
                if (range1.IsDisjointWith(range2))
                {
                    var error = true;
                }

                throw new ArgumentException("Gene ranges are overlapped!");
            }
            var temp = new int[genes.Length];
            var range1EndPosition = range1.Position + range1.Length;
            var range2EndPosition = range2.Position + range2.Length;
            var middleLength = range2.Position - range1EndPosition;

            if (range1.Position > 0)
            {
                Array.Copy(genes, 0, temp, 0, range1.Position);
            }
            Array.Copy(genes, range2.Position, temp, range1.Position, range2.Length);
            if (middleLength > 0)
            {
                Array.Copy(genes, range1EndPosition, temp, range1.Position + range2.Length, middleLength);
            }
            Array.Copy(genes, range1.Position, temp, range2EndPosition - range1.Length, range1.Length);
            if (range2EndPosition < genes.Length)
            {
                Array.Copy(genes, range2EndPosition, temp, range2EndPosition, genes.Length - range2EndPosition);
            }

            return temp;
        }
    }
}
