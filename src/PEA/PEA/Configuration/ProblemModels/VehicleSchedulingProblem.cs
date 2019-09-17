using System;

namespace Pea.Configuration.ProblemModels
{
    public class VehicleSchedulingProblem : IProblemModel
    {
        public int TripsCount { get; set; }

        public VehicleSchedulingProblem(int tripsCount)
        {
            TripsCount = tripsCount;
        }

        public SubProblemBuilder Apply(string key, SubProblemBuilder subProblemBuilder)
        {
            return subProblemBuilder
                .WithEncoding<Chromosome.SortedSubset>(key)
                .WithAlgorithm<Algorithm.SteadyState>()
                .SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, TripsCount)
                .SetParameter(Algorithm.ParameterNames.MutationProbability, 0.7)
                .SetParameter(Selection.ParameterNames.TournamentSize, 2)
                .SetParameter(Core.Island.ParameterNames.IslandsCount, Math.Floor(Math.Sqrt(TripsCount)));

            /*                .WithAlgorithm<Algorithm.SteadyState>()
                .AddChromosome<Pea.Chromosome.SortedSubset>("VehicleScheduling")
                .WithConflictDetector<VSConflictDetector>()
                .WithCreator<VSEntityCreator>()
                .WithEvaluation<VSEvaluation>()

                .WithFitness<Pea.Fitness.ParetoMultiobjective>()
                .AddSelection<Pea.Selection.TournamentSelection>()
                .AddReinsertion<Pea.Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion>()

                .SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, 2500)
                .SetParameter(Algorithm.ParameterNames.MutationProbability, 0.5)
                .SetParameter(Pea.Selection.ParameterNames.TournamentSize, 2)
                .SetParameter(Island.ParameterNames.ArchipelagosCount, 1)
                .SetParameter(Island.ParameterNames.IslandsCount, 1)
                .SetParameter(Island.ParameterNames.EvaluatorsCount, 2)
                .SetParameter(Chromosome.ParameterNames.ConflictReducingProbability, 0.5)
                .SetParameter(Chromosome.ParameterNames.FailedCrossoverRetryCount, 20)
                .SetParameter(Chromosome.ParameterNames.FailedMutationRetryCount, 10);
                */
        }
    }
}
