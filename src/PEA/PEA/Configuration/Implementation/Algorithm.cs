using System;

namespace Pea.Configuration
{
    public class Algorithm
    {
        public Type AlgorithmType { get; set; }

        public Algorithm(Type algorithmType)
        {
            AlgorithmType = algorithmType;
        }
    }
}
