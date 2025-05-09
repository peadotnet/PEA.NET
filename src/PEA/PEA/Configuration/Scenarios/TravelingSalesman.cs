using System;

namespace Pea.Configuration.ProblemModels
{
    public class TravelingSalesman : IScenario
    {
        public int Size { get; }

        public TravelingSalesman(int size)
        {
            Size = size;
        }

        public SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder)
        {
            return subProblemBuilder
                .WithEncoding<Chromosome.Permutation>(key)
                .WithAlgorithm<Algorithm.SteadyState>()
                .SetParameter(Algorithm.ParameterNames.PopulationSize, Size * 10) //*2
                .SetParameter(Chromosome.ParameterNames.MutationProbability, 0.1)
                .SetParameter(Algorithm.ParameterNames.FitnessLength, 1)
                .SetParameter(Population.ParameterNames.TournamentSize, 2)
                .SetParameter(Core.Island.ParameterNames.IslandsCount, 1)
                //.SetParameter(Core.Island.ParameterNames.IslandsCount, Math.Floor(Math.Sqrt(Size)))
                .SetParameter("ProblemSize", Size);
        }
    }
}
