using System;
using System.Collections.Generic;

namespace Pea.Configuration.Implementation
{
    public class Encoding
    {
        public string Key { get; set; }

        public Type ChromosomeType { get; set; }

        public Algorithm Algorithm { get; set; }

        public List<IBuildAction> Creators = new List<IBuildAction>();

        public List<IBuildAction> Operators = new List<IBuildAction>();
    }
}
