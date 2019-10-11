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
                .SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, TripsCount / 10)
                .SetParameter(Algorithm.ParameterNames.MutationProbability, 0.7)
                .SetParameter(Selection.ParameterNames.TournamentSize, 2)
                .SetParameter("ProblemSize", TripsCount)
                .SetParameter(Core.Island.ParameterNames.IslandsCount, 1);// Math.Floor(Math.Sqrt(TripsCount)));


                //.WithConflictDetector<VSConflictDetector>()
                //.WithCreator<VSEntityCreator>()

                //.WithFitness<Pea.Fitness.ParetoMultiobjective>()
                //.AddSelection<Pea.Selection.TournamentSelection>()
                //.AddReinsertion<Pea.Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion>()

            //.WithConflictDetector<VSConflictDetector>()
            //.WithCreator<VSEntityCreator>()
            //.WithEvaluation<VSEvaluation>()

            //.WithFitness<Pea.Fitness.ParetoMultiobjective>()
            //.AddSelection<Pea.Selection.TournamentSelection>()
            //.AddReinsertion<Pea.Reinsertion.ReplaceWorstParentWithBestChildrenReinsertion>()

            //.SetParameter(Algorithm.ParameterNames.MaxNumberOfEntities, 2500) //->ProblemType
            //.SetParameter(Algorithm.ParameterNames.MutationProbability, 0.5) //->ProblemType
            //.SetParameter(Pea.Selection.ParameterNames.TournamentSize, 2)  //->Algorithm
            //.SetParameter(Island.ParameterNames.ArchipelagosCount, 1)
            //.SetParameter(Island.ParameterNames.IslandsCount, 1)
            //.SetParameter(Island.ParameterNames.EvaluatorsCount, 2) //-> Algorithm
            //.SetParameter(Chromosome.ParameterNames.ConflictReducingProbability, 0.5) //->ChromosomeFactory
            //.SetParameter(Chromosome.ParameterNames.FailedCrossoverRetryCount, 20) //->ChromosomeFactory
            //.SetParameter(Chromosome.ParameterNames.FailedMutationRetryCount, 10); //->ChromosomeFactory

        }
    }
}
