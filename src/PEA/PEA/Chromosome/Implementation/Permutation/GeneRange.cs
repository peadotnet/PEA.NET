using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class GeneRange
    {
        public int Position { get; set; }
        public int Length { get; set; }
        public int End => Length + Position - 1;

        public GeneRange(int position, int length)
        {
            Position = position;
            Length = length;
        }

        public bool IsDisjointWith(GeneRange other)
        {
            var thisEnd = this.Position + this.Length;
            var otherEnd = other.Position + other.Length;
            return thisEnd < other.Position || otherEnd < this.Position; 
        }

        public bool ContainsPosition(int position)
        {
            return this.Position <= position && this.Position + this.Length > position;
        }
    }
}
