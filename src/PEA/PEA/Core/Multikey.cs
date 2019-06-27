using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public class MultiKey
    {
        public string[] Keys { get; }

        public int Count => Keys.Length;

        public string this[int i] => this.Keys[i];

        public MultiKey(params string[] keys)
        {
            Keys = keys;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((ICollection<string>)Keys).GetEnumerator();
        }

        public bool Contains(string item)
        {
            return Keys.Contains(item);
        }
    }
}
