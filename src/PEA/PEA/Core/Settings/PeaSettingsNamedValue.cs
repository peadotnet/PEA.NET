using System;

namespace Pea.Core.Settings
{
    [Obsolete]
    public class PeaSettingsNamedValue_old
    {
        public string Name { get; }
        public double Value { get; }

        public PeaSettingsNamedValue_old(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
