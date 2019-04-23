namespace Pea.Core
{
    public interface IStochasticProvider<T>
    {
        T ChooseOne();
        IStochasticProvider<T> Add(T item, double probability);
    }
}
