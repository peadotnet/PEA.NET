using System;

namespace Pea.Core.Settings
{
    public class PeaSettingsTypeProbability
    {
        public Type ValueType { get; }
        public double Probability { get; }

        public PeaSettingsTypeProbability(Type valueType, double probability)
        {
            ValueType = valueType;
            Probability = probability;
        }
    }
}
