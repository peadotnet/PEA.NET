using System;
using System.Collections.Generic;

namespace Pea.Core
{
    public class ParameterSet : IParameterSet
    {
        private Dictionary<string, double> Parameters { get; } = new Dictionary<string, double>();

        public ParameterSet() { }

        public ParameterSet(Dictionary<string, double> parameters) : this()
        {
            Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        }

        public double GetValue(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

            return Parameters[parameterKey];
        }

        public int GetInt(string parameterKey)
        {
            if (!Parameters.ContainsKey(parameterKey)) throw new ArgumentException(nameof(parameterKey));

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
    }
}
