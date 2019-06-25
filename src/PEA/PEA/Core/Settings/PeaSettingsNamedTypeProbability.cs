using System;

namespace Pea.Core.Settings
{
    public class PeaSettingsNamedTypeProbability
    {
        public string[] Names { get; }
        public Type ValueType { get; }
        public double Probability { get; }

        public PeaSettingsNamedTypeProbability(string[] names, Type valueType, double probability)
        {
            Names = names;
            ValueType = valueType;
            Probability = probability;
        }
    }
}
