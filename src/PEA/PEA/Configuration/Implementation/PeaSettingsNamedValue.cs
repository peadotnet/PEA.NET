namespace Pea.Configuration.Implementation
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
