namespace Pea.Chromosome.Implementation.SortedSubset
{
    public class GeneRegion : GenePosition
    {
        public int Length { get; set; }
         
        public GeneRegion(int section, int position, int length) : base(section, position)
        {
            Length = length;
        }
    }
}
