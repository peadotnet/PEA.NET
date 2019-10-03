using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public class ParameterSet : IParameterSet
    {
        private Dictionary<string, double> Parameters { get; } = new Dictionary<string, double>();

        public ParameterSet() { }

        public ParameterSet(ParameterSet parameters) : this()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters.Parameters)
            {
                SetValue(parameter.Key, parameter.Value);
            }
        }

        public ParameterSet(IEnumerable<PeaSettingsNamedValue> parameters) : this()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                SetValue(parameter.Name, parameter.Value);
            }
        }

        public double GetValue(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

            return Parameters[parameterKey];
        }

        public IEnumerable<KeyValuePair<string, double>> GetAllValues()
        {
            var parameters = new List<KeyValuePair<string, double>>();

            foreach (var parameter in Parameters)
            {
                parameters.Add(new KeyValuePair<string, double>(parameter.Key, parameter.Value));
            }

            return parameters;
        }

        public int GetInt(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey) + $": {parameterKey}");

            return Convert.ToInt32(Parameters[parameterKey]);
        }

        public void SetValue(string parameterKey, double newValue)
        {
            if (!Parameters.ContainsKey(parameterKey))
            {
                Parameters.Add(parameterKey, newValue);
            }
            else
            {
                Parameters[parameterKey] = newValue;
            }
        }

        public void SetValueRange(IEnumerable<KeyValuePair<string, double>> parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                SetValue(parameter.Key, parameter.Value);
            }
        }

        public void SetValueRange(IParameterSet parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            SetValueRange(parameters.GetAllValues());
        }
    }
}
