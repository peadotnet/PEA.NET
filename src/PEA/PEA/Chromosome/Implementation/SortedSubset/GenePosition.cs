namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class GenePosition
    {
        public int Section { get; set; }
        public int Position { get; set; }

        public GenePosition(int section, int position)
        {
            Section = section;
            Position = position;
        }
    }
}
