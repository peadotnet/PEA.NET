using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core.Entity;
using Pea.Population.Replacement;
using Pea.Selection;

namespace Pea.Core.Island
{
    public static class IslandEngineFactory
    {
        public static IslandEngine Create(string islandName, PeaSettings settings, int seed)
        {
            var islandKey = new MultiKey(islandName);
            return Create(islandKey, settings, seed);
        }

        public static IslandEngine Create(MultiKey islandKey, PeaSettings settings, int seed)
		{
            var parameterSet = CreateParameters(settings);

            if (seed == 0) seed = islandKey.GetHashCode() + Environment.TickCount;
			var random = (IRandom)Activator.CreateInstance(settings.Random, seed);

			var fitness = (IFitnessFactory)Activator.CreateInstance(settings.Fitness);
			var fitnessComparer = fitness.GetFitnessComparer();

			var engine = new IslandEngine(random, settings, parameterSet);

            var algorithm = CreateAlgorithm(engine, settings);
            var algParameters = algorithm.GetParameters();
            parameterSet.SetValueRange(algParameters, ParameterSource.AlgorithmDefault);

            var conflictDetectors = CreateConflictDetectors(settings.SubProblemList);

			var chromosomeFactories = CreateChromosomeFactories(engine, settings, conflictDetectors, random);
			var defaultCreator = new EntityCreator(settings.EntityType, chromosomeFactories, random);

			IMigrationStrategy migrationStrategy = CreateMigrationStrategy(engine, random, fitnessComparer, parameterSet, settings);

            var entityCreators = CreateEntityCreators(settings.SubProblemList, defaultCreator, random);

            //TODO: Abstract factory...
            engine.Algorithm = algorithm.GetAlgorithm(parameterSet, entityCreators, engine.MergeToBests);
            engine.Algorithm.Selections = CreateSelections(algorithm, settings, parameterSet, random, fitnessComparer);
            engine.Algorithm.Replacements = CreateReinsertions(algorithm, settings, parameterSet, random, fitnessComparer);
            engine.Algorithm.EntityMutation = new EntityMutation(chromosomeFactories, random);
            engine.Algorithm.EntityCrossover = new EntityCrossover(chromosomeFactories, random);

            engine.Algorithm.FitnessComparer = fitnessComparer;
			engine.ConflictDetectors = conflictDetectors;
			engine.MigrationStrategy = migrationStrategy;

			engine.Reduction = new Population.Reduction.CleanOutTournamentLosers(random, parameterSet);
			//engine.Reduction = new Population.Reduction.DoNothingReduction();


			engine.Algorithm.StopCriteria = settings.StopCriteria;
            engine.RestartStrategy = settings.RestartStrategy;

            return engine;
		}

		private static IDictionary<string, IChromosomeFactory> CreateChromosomeFactories(IEngine engine, PeaSettings settings, IDictionary<string, IList<IConflictDetector>> conflictDetectors, IRandom random)
        {
            var factories = new Dictionary<string, IChromosomeFactory>();

            foreach (var subProblem in settings.SubProblemList)
            {
                var parameterSet = new ParameterSet(subProblem.ParameterSet, ParameterSource.SubproblemDefault);
                var factoryInstance = Activator.CreateInstance(subProblem.Encoding.ChromosomeType, random,
                    parameterSet, conflictDetectors[subProblem.Encoding.Key]) as IChromosomeFactory;

                factoryInstance.Apply(engine);

                factories.Add(subProblem.Encoding.Key, factoryInstance);
            }

            return factories;
        }

        private static ParameterSet CreateParameters(PeaSettings settings)
        {
            var parameterSet = new ParameterSet(settings.ParameterSet, ParameterSource.UserSetting);

			foreach (var subProblem in settings.SubProblemList)
            {
                parameterSet.SetValueRange(subProblem.ParameterSet, ParameterSource.UserSetting);
            }
            return parameterSet;
        }

        private static IDictionary<string, IList<IConflictDetector>> CreateConflictDetectors(List<SubProblem> subProblemList)
        {
            var result = new Dictionary<string, IList<IConflictDetector>>();
            foreach (var subProblem in subProblemList)
            {
                string key = subProblem.Encoding.Key;
                if (!result.ContainsKey(key))
                {
                    var detectors = new List<IConflictDetector>();
                    foreach (var detectorType in subProblem.ConflictDetectors)
                    {
                        var detectorInstance = (IConflictDetector)Activator.CreateInstance(detectorType);
                        detectors.Add(detectorInstance);
                    }
                    result.Add(key, detectors);
                }
            }
            return result;
        }

        private static IProvider<IEntityCreator> CreateEntityCreators(List<SubProblem> subProblems, EntityCreator defaultCreator, IRandom random)
        {
            List<IEntityCreator> creators = new List<IEntityCreator>();
            var result = new Dictionary<string, IList<IEntityCreator>>();
            foreach (var subProblem in subProblems)
            {
                string key = subProblem.Encoding.Key;

                foreach (var creatorType in subProblem.EntityCreators)
                {
                    var creatorInstance = (IEntityCreator)Activator.CreateInstance(creatorType, random);
                    creators.Add(creatorInstance); 
                }
            }

            if (creators.Count == 0) creators.Add(defaultCreator);

            var provider = CreateProvider<IEntityCreator>(creators.Count, random);
            foreach(var creator in creators)
			{
                provider.Add(creator, 1.0);
			}
            return provider;
        }

        private static IAlgorithmFactory CreateAlgorithm(IEngine engine, PeaSettings settings)
        {
            //TODO: choose first matching algorithm type from chromosomeFactories, or apply settings
            return new Algorithm.SteadyState();
        }

        private static IMigrationStrategy CreateMigrationStrategy(IslandEngine engine, IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters, PeaSettings settings)
        {
            //TODO: MigrationFactory!
            var selection = new TournamentSelection(random, fitnessComparer, parameters);
            var replacement = new ReplaceWorstEntitiesOfPopulation(random, fitnessComparer, parameters);
            var strategy = new Migration.Implementation.MigrationStrategy(random, selection, replacement, engine.Parameters);
            strategy.Parameters.SetValue(Migration.ParameterNames.MigrationReceptionRate, 0.01, ParameterSource.EngineDefault);
            return strategy;
        }

        public static IProvider<ISelection> CreateSelections(IAlgorithmFactory algorithm, PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var selections = algorithm.GetSelections();

            //TODO: impement override by settings

            var selectionProvider = CreateProvider<ISelection>(selections.Count, random);

            foreach (var selectionType in selections)
            {
                var selectionInstance = (ISelection)Activator.CreateInstance(selectionType, random, fitnessComparer, parameterSet);
                selectionProvider.Add(selectionInstance, 1.0);
            }

            return selectionProvider;
        }

        private static IProvider<IReplacement> CreateReinsertions(IAlgorithmFactory algorithm, PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var reinsertions = algorithm.GetReinsertions();

            //TODO: impement override by settings


            var reinsertionProvider = CreateProvider<IReplacement>(reinsertions.Count, random);

            foreach (var reinsertion in reinsertions)
            {
                var reinsertionInstance = (IReplacement)Activator.CreateInstance(reinsertion, random, fitnessComparer, parameterSet);
                reinsertionProvider.Add(reinsertionInstance, 1.0);
            }

            return reinsertionProvider;
        }

        public static IProvider<T> CreateProvider<T>(int count, IRandom random)
        {
            if (count < 2) return new SimpleProvider<T>();

            return new StochasticProvider<T>(random);
        }

    }
}
