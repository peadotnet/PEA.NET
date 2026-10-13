namespace Pea.Chromosome
{
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}