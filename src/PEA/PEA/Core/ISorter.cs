using System.Collections.Generic;

namespace Pea.Core
{
    public interface ISorter<T>
    {
        bool IsSorted(IList<T> list, int firstIndex, int lastIndex, IComparer<T> comparer=null);
        void Sort(IList<T> list, IComparer<T> comparer, int firstIndex, int lastIndex);
    }
}
