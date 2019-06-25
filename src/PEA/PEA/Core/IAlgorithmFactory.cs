using Pea.Algorithm;
using Pea.Algorithm.Implementation;
using Pea.Island;

namespace Pea.Core
{
    public interface IAlgorithmFactory
    {
        IAlgorithm GetAlgorithm(IPopulation population, IEngine engine,
            AlgorithmBase.DecodePhenotypesDelegate decodePhenotypes,
            AlgorithmBase.AssessFitnessDelegate assessFitness);
    }
}
