using System;
using System.Collections.Generic;

namespace Pea.Configuration.Implementation
{
    public class Encoding
    {
        public string Key { get; set; }

        public Type ChromosomeType { get; set; }

        public List<BuildAction<Algorithm>> Algorithms = new List<BuildAction<Algorithm>>();

        public List<BuildAction<Type>> Creators = new List<BuildAction<Type>>();

        public List<BuildAction<Type>> Operators = new List<BuildAction<Type>>();
    }
}
