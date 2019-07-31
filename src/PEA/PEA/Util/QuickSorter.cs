using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Util
{
    public class QuickSorter<T> : ISorter<T>
    {
        public bool IsSorted(IList<T> list, int firstIndex, int lastIndex, IComparer<T> comparer = null)
        {
            if ((comparer == null) && !typeof(IComparable).IsAssignableFrom(typeof(T))) throw new ArgumentNullException(nameof(comparer));

            for (var i = firstIndex + 1; i < lastIndex; i++)
            {
                if (Compare(comparer, list[i - 1], list[i]) > 0) return false;
            }
            return true;
        }

        public void Sort(IList<T> list, IComparer<T> comparer, int firstIndex, int lastIndex)
        {
            if (!IsSorted(list, firstIndex, lastIndex, comparer))
            {
                QuickSort(list, comparer, firstIndex, lastIndex);
            }
        }

        private static void QuickSort(IList<T> list, IComparer<T> comparer, int first, int last)
        {
            var stack = new int[last - first + 1];

            var top = -1;

            stack[++top] = first;
            stack[++top] = last;

            while (top >= 0)
            {
                last = stack[top--];
                first = stack[top--];

                var p = Partition(list, comparer, first, last);

                if (p - 1 > first)
                {
                    stack[++top] = first;
                    stack[++top] = p - 1;
                }

                if (p + 1 < last)
                {
                    stack[++top] = p + 1;
                    stack[++top] = last;
                }
            }
        }

        private static int Partition(IList<T> list, IComparer<T> comparer, int first, int last)
        {
            T temp;
            var pivot = list[last];

            var i = (first - 1);
            for (var j = first; j <= last - 1; j++)
            {
                if (Compare(comparer, list[j], pivot) > 0) continue;

                i++;
                temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }

            temp = list[i + 1];
            list[i + 1] = list[last];
            list[last] = temp;

            return i + 1;
        }

        private static int Compare(IComparer<T> comparer, T firstValue, T lastValue)
        {
            if (comparer == null)
            {
                return ((IComparable) firstValue).CompareTo(lastValue);
            }
            else
            {
                return comparer.Compare(firstValue, lastValue);
            }
        }
    }
}
