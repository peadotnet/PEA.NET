using System;
using System.Collections.Generic;
using System.Linq;

namespace Pea.Core
{
    public struct MultiKey : IEquatable<MultiKey>
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

        public bool Equals(MultiKey other)
        {
            if (other.Count != this.Count) return false;

            for (int i = 0; i < Keys.Length; i++)
            {
                if (Keys[i] != other.Keys[i]) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            foreach (var key in Keys)
            {
                hashCode += key.GetHashCode();
            }

            return hashCode;
        }
    }
}
