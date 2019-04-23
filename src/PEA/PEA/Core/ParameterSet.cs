using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public class ParameterSet
    {
        private Dictionary<string, float> Parameters { get; } = new Dictionary<string, float>();

        public ParameterSet() { }

        public ParameterSet(Dictionary<string, float> parameters) : this()
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public float GetValue(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

            return Parameters[parameterKey];
        }

        public void SetValue(string parameterKey, float newValue)
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
    }
}
