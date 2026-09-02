using Pea.Configuration.Implementation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Pea.Core
{
    public class ParameterSet : IParameterSet
    {
        private class ParameterValueWithSource
        {
            public double Value { get; set; }
            public ParameterSource Source { get; set; }

            public ParameterValueWithSource(double value,  ParameterSource source)
            {  
                Value = value; 
                Source = source; 
            }
        }

        private Dictionary<string, ParameterValueWithSource> Parameters { get; } = new Dictionary<string, ParameterValueWithSource>();

        public ParameterSet() { }

        public ParameterSet(ParameterSet parameters, ParameterSource source) : this()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters.Parameters)
            {
                SetValue(parameter.Key, parameter.Value.Value, parameter.Value.Source);
            }
        }

        public ParameterSet(IEnumerable<PeaSettingsNamedValue> parameters, ParameterSource source) : this()
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                SetValue(parameter.Name, parameter.Value, source);
            }
        }

        public double GetValue(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

            return Parameters[parameterKey].Value;
        }

        public IEnumerable<PeaSettingsNamedValue> GetAllValues()
        {
            var parameters = new List<PeaSettingsNamedValue>(Parameters.Count);

            foreach (var parameter in Parameters)
            {
                parameters.Add(new PeaSettingsNamedValue(parameter.Key, parameter.Value.Value));
            }

            return parameters;
        }

        public int GetInt(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey) + $": {parameterKey}");

            return Convert.ToInt32(Parameters[parameterKey].Value);
        }

        public void SetValue(string key, double newValue, ParameterSource source)
        {
            if (!Parameters.TryGetValue(key, out var existing) || source >= existing.Source)
            {
                Parameters[key] = new ParameterValueWithSource(newValue, source);
            }
        }

        //public void SetValueRange(IEnumerable<KeyValuePair<string, double>> parameters)
        //{
        //    if (parameters == null) throw new ArgumentNullException(nameof(parameters));

        //    foreach (var parameter in parameters)
        //    {
        //        SetValue(parameter.Key, parameter.Value);
        //    }
        //}

        public void SetValueRange(IEnumerable<PeaSettingsNamedValue> parameters, ParameterSource source)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach (var parameter in parameters)
            {
                SetValue(parameter.Name, parameter.Value, source);
            }
        }

        public void SetValueRange(ParameterSet parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            foreach(var parameter in parameters.Parameters)
            {
                SetValue(parameter.Key, parameter.Value.Value, parameter.Value.Source);
            }
        }
    }
}
