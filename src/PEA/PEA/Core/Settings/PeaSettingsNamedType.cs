using System;

namespace Pea.Core.Settings
{
    public class PeaSettingsNamedType
    {
        public string[] Names { get; }
        public Type ValueType { get; }

        public PeaSettingsNamedType(string[] names, Type valueType)
        {
            Names = names;
            ValueType = valueType;
        }

        public PeaSettingsNamedType(string name, Type valueType)
        {
            Names = new string[] {name};
            ValueType = valueType;
        }
    }
}
