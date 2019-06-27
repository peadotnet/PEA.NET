using Pea.Algorithm.Implementation;

namespace Pea.Core
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(IEngine engine, AlgorithmBase.EvaluationDelegate evaluation);
    }
}
