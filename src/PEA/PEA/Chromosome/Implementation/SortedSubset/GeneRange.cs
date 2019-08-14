namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class GeneRange
    {
        public int Section { get; set; }
        public int FirstPosition { get; set; }
        public int LastPosition { get; set; }

        public int Length => LastPosition - FirstPosition;

        public GeneRange(int section, int firstPosition, int lastPosition)
        {
            Section = section;
            FirstPosition = firstPosition;
            LastPosition = lastPosition;
        }
    }
}
