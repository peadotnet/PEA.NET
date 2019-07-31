using System.Collections.Generic;
using Pea.Core;

namespace Pea.Chromosome.Implementation.SortedSubset
{
    //TODO: just for testing purpose; delete this class!
    public static class SortedSubsetChromosomeValidator
    {
        public static int EntityCount = 0;

        public static IConflictDetector ConflictDetector { get; set; }

        public static List<GenePosition> SearchForConflict(int[][] sections)
        {
            List<GenePosition> positions = new List<GenePosition>();

            for (int s = 0; s < sections.Length; s++)
            {
                for (int p = 0; p < sections[s].Length - 1; p++)
                {
                    if (ConflictDetector.ConflictDetected(sections[s][p], sections[s][p + 1]))
                    {
                        positions.Add(new GenePosition(s, p));
                    }
                }
            }

            return positions;
        }
    }
}
