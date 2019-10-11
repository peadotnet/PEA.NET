using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public interface IParameterSet
    {
        double GetValue(string parameterKey);
        IEnumerable<PeaSettingsNamedValue> GetAllValues();
        int GetInt(string parameterKey);
        void SetValue(string parameterKey, double newValue);
        void SetValueRange(IEnumerable<PeaSettingsNamedValue> parameters);
    }
}