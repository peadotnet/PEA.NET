namespace Pea.Core
{
    public class SimpleProvider<T> : IProvider<T>
    {
        private T Item { get; set; }

        public T GetOne()
        {
            return Item;
        }

        public IProvider<T> Add(T item, double probability)
        {
            Item = item;
            return this;
        }
    }
}
