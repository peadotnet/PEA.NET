using Pea.Algorithm.Implementation;
using Pea.Core;

namespace Pea.Algorithm
{
    public class SteadyState : IAlgorithmFactory
    {
        public IAlgorithm GetAlgorithm(IPopulation population, IEngine engine, AlgorithmBase.DecodePhenotypesDelegate decodePhenotypes,
            AlgorithmBase.AssessFitnessDelegate assessFitness)
        {
            return new SteadyStateAlgorithm(population, engine, decodePhenotypes, assessFitness);
        }
    }
}
