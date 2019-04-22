using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public class ParameterSet
    {
        private Dictionary<string, float> _parameters { get; } = new Dictionary<string, float>();

        public ParameterSet() { }

        public ParameterSet(Dictionary<string, float> parameters) : this()
        {
            _parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public float GetValue(string parameterKey)
        {
            if (!_parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

            return _parameters[parameterKey];
        }

        public void SetValue(string parameteKey, float newValue)
        {
            if (!_parameters.ContainsKey(parameteKey))
            {
                _parameters.Add(parameteKey, newValue);
            }
            else
            {
                _parameters[parameteKey] = newValue;
            }
        }
    }
}
