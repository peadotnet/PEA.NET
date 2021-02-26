using System.Collections.Generic;

namespace Pea.Core
{
    public interface IProvider<T> : IEnumerable<T>
    {
        T GetOne();
        IProvider<T> Add(T item, double probability);
    }
}
