using System.Collections.Generic;

namespace Pea.Rnd
{
    public interface IProvider<T> : IEnumerable<T>
    {
        T GetOne();
        IProvider<T> Add(T item, double probability);
    }
}
