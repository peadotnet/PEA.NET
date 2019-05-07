namespace Pea.Core
{
    public interface IParameterSet
    {
        double GetValue(string parameterKey);
        int GetInt(string parameterKey);
        void SetValue(string parameterKey, double newValue);
    }
}