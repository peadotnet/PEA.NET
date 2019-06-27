using Pea.Algorithm.Implementation;
using Pea.Core;

namespace Pea.Algorithm
{
    public class SteadyState : IAlgorithmFactory
    {
        public IAlgorithm GetAlgorithm(IEngine engine,
            AlgorithmBase.EvaluationDelegate evaluation)
        {
            return new SteadyStateAlgorithm(engine, evaluation);
        }
    }
}
