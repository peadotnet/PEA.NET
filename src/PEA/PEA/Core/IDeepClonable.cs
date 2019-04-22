namespace Pea.Core
{
    public interface IDeepCloneable<out T>
    {
        T DeepClone();
    }
}