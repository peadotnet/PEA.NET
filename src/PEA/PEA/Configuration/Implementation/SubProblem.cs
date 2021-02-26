using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Configuration.Implementation
{
    public class SubProblem
    {
        public Encoding Encoding { get; set; }

        public List<PeaSettingsNamedValue> ParameterSet { get; set; } = new List<PeaSettingsNamedValue>();

        public List<Type> ConflictDetectors { get; set; } = new List<Type>();

        public List<Type> EntityCreators { get; set; } = new List<Type>();

        public Type Decoder { get; set; }

        public Type Niching { get; set; }
    }
}
