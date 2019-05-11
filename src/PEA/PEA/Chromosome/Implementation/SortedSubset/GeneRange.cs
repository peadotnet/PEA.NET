namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class GeneRange : GenePosition
    {
        public int Length { get; set; }
         
        public GeneRange(int section, int position, int length) : base(section, position)
        {
            Length = length;
        }

        public GeneRange(GenePosition genePosition, int length) : base(genePosition.Section, genePosition.Position)
        {
            Length = length;
        }
    }
}
