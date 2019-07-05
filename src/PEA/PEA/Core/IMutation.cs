namespace Pea.Core
{
    public interface IMutation
    {
        IChromosome Mutate(IChromosome chromosome);
    }

    public interface IMutation<TC> : IMutation where TC: IChromosome
    {
        new IChromosome Mutate(IChromosome chromosome);
    }
}
