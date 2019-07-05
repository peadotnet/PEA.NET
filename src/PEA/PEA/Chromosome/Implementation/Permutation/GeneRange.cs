using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Chromosome.Implementation.Permutation
{
    public class GeneRange
    {
        public int Position { get; set; }
        public int Length { get; set; }

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
    }
}
