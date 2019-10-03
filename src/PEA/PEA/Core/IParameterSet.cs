using System.Collections.Generic;

namespace Pea.Core
{
    public interface IParameterSet
    {
        double GetValue(string parameterKey);
        IEnumerable<KeyValuePair<string, double>> GetAllValues();
        int GetInt(string parameterKey);
        void SetValue(string parameterKey, double newValue);
        void SetValueRange(IEnumerable<KeyValuePair<string, double>> parameters);
    }
}