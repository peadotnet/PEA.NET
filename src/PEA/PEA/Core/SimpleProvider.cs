using System.Collections;
using System.Collections.Generic;

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

		public IEnumerator<T> GetEnumerator()
		{
            var enumerable = new List<T>(1) { Item };
            return enumerable.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
            var enumerable = new List<T>(1) { Item };
            return enumerable.GetEnumerator();
        }
    }
}
