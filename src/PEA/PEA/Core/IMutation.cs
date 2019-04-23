namespace Pea.Core
{
    public interface IMutation<TC> where TC: IChromosome
    {
        TC Mutate(TC chromosome);
    }
}
