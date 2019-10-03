using System;

namespace Pea.Configuration.ProblemModels
{
    public class TravelingSalesmanProblem : IProblemModel
    {
        public int Size { get; }

        public TravelingSalesmanProblem(int size)
        {
            Size = size;
        }

        public SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder)
        {
            return subProblemBuilder
                .WithEncoding<Chromosome.Permutation>(key)
                .WithAlgorithm<Algorithm.SteadyState>()
                //Fitness?
                .SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, Size * 2)
                .SetParameter(Algorithm.ParameterNames.MutationProbability, 0.7)
                .SetParameter(Selection.ParameterNames.TournamentSize, 2)
                .SetParameter("ProblemSize", Size)
                .SetParameter(Core.Island.ParameterNames.IslandsCount, Math.Floor(Math.Sqrt(Size)));
        }
    }
}
