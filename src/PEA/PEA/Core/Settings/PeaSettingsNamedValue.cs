namespace Pea.Core.Settings
{
    public class PeaSettingsNamedValue
    {
        public string Name { get; }
        public double Value { get; }

        public PeaSettingsNamedValue(string name, double value)
        {
            Name = name;
            Value = value;
        }
    }
}
