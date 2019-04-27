namespace Pea.Core
{
    public interface IPhenotypeDecoder
    {
        IPhenotype Decode(IGenotype genotype);
    }
}
