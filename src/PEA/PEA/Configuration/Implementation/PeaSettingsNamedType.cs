using System;

namespace Pea.Configuration.Implementation
{
    public class PeaSettingsNamedType
    {
        public MultiKey Keys { get; }
        public Type ValueType { get; }

        public PeaSettingsNamedType(MultiKey keys, Type valueType)
        {
            Keys = keys;
            ValueType = valueType;
        }

        public PeaSettingsNamedType(string key, Type valueType)
        {
            Keys = new MultiKey(key);
            ValueType = valueType;
        }
    }
}
