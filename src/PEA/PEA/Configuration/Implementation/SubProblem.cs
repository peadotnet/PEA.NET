using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Configuration.Implementation
{
    public class SubProblem
    {
        public Encoding Encoding { get; set; }

        public ParameterSet ParameterSet { get; set; } = new ParameterSet();

        public List<Type> ConflictDetectors { get; set; } = new List<Type>();

        public Type Decoder { get; set; }

        public Type Niching { get; set; }
    }
}
