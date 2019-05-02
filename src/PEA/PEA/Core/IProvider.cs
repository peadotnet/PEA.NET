namespace Pea.Core
{
    public interface IProvider<T>
    {
        T GetOne();
        IProvider<T> Add(T item, double probability);
    }
}
