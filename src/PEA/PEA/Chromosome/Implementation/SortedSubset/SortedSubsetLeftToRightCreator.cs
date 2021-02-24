using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class SortedSubsetLeftToRightCreator : IChromosomeCreator<SortedSubsetChromosome>
    {
        public double GreedyProbability { get; set; } = 0.99;

        public int Size { get; }
        public IRandom Random { get; }
        public IList<INeighborhoodConflictDetector> ConflictDetectors { get; }

        //TODO: costdetectors
        public SortedSubsetLeftToRightCreator(int size, IRandom random, IList<INeighborhoodConflictDetector> conflictDetectors)
        {
            Size = size;
            Random = random;
            ConflictDetectors = conflictDetectors;
        }

        public virtual IChromosome Create()
        {
            List<List<int>> indices = new List<List<int>>();

            for (int i = 0; i < Size; i++)
            {
                int targetIndex;

                targetIndex = ChooseTarget(indices, i);

                if (targetIndex == indices.Count)
                {
                    indices.Add(new List<int>());
                }

                indices[targetIndex].Add(i);
            }

            int[][] geneSections = CreateSectionsFromList(indices);
            var chromosome = new SortedSubsetChromosome(geneSections);
            return chromosome;
        }

        private int ChooseTarget(List<List<int>> indices, int i)
        {
            int targetIndex;
            var fitVehicles = CheckFitSections(indices, i);

            if (fitVehicles.Count == 0)
            {
                targetIndex = indices.Count;
            }
            else
            {
                var randomIndex = Random.GetInt(0, fitVehicles.Count);
                targetIndex = fitVehicles[randomIndex];
            }

            return targetIndex;
        }

        private List<int> CheckFitSections(List<List<int>> indices, int i)
        {
            var fitSections = new List<int>();

            for (int s = 0; s < indices.Count; s++)
            {
                var previousIndex = indices[s][indices[s].Count - 1];

                if (!ConflictDetectors[0].ConflictDetected(previousIndex, i))   //TODO: multiple conflict detectors
                {
                    fitSections.Add(s);
                }
            }

            return fitSections;
        }

        private static int[][] CreateSectionsFromList(List<List<int>> indices)
        {
            var geneSections = new int[indices.Count][];
            for (int s = 0; s < indices.Count; s++)
            {
                geneSections[s] = indices[s].ToArray();
            }

            return geneSections;
        }
    }
}
