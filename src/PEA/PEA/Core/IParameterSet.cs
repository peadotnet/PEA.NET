using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public enum ParameterSource
    {
        EngineDefault = 0,
        AlgorithmDefault = 1,
        SubproblemDefault = 2,
        ChromosomeOperationDefault = 3,
        UserSetting = 4,
        PersistentSettings = 5
    }

    public interface IParameterSet
    {
        double GetValue(string parameterKey);
        IEnumerable<PeaSettingsNamedValue> GetAllValues();
        int GetInt(string parameterKey);
        void SetValue(string parameterKey, double newValue, ParameterSource source);
        void SetValueRange(IEnumerable<PeaSettingsNamedValue> parameters, ParameterSource source);
    }
}