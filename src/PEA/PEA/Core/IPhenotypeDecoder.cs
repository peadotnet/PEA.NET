namespace Pea.Core
{
    public interface IPhenotypeDecoder
    {
        void Init(IPhenotypeDecoderInitData initData);
        IPhenotype Decode(IGenotype genotype);
    }
}
