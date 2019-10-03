using System;
using System.Collections.Generic;
using Pea.Configuration.Implementation;
using Pea.Core.Entity;

namespace Pea.Core.Island
{
    public static class IslandEngineFactory
    {
        public static IslandEngine Create(PeaSettings settings)
        {
            var random = (IRandom)Activator.CreateInstance(settings.Random);
            var parameterSet = CreateParameters(settings);

            var fitness = (IFitnessFactory)Activator.CreateInstance(settings.Fitness);
            var fitnessComparer = fitness.GetFitnessComparer();

            var engine = new IslandEngine();

            var algorithm = CreateAlgorithm(engine, settings);

            engine.Random = random;
            engine.Settings = settings;
            engine.Parameters = parameterSet;

            engine.Algorithm = algorithm.GetAlgorithm(engine);

            engine.FitnessComparer = fitnessComparer;
            engine.ConflictDetectors = CreateConflictDetectors(settings.SubProblemList);
            engine.Selections = CreateSelections(algorithm, settings, parameterSet, random, fitnessComparer);
            engine.Reinsertions = CreateReinsertions(algorithm, settings, parameterSet, random, fitnessComparer);

            engine.Parameters.SetValueRange(algorithm.GetParameters());

            engine.EntityCreator = new EntityCreator(settings.EntityType, settings.SubProblemList, engine.ConflictDetectors, random);
            //engine.EntityCreators = CreateEntityCreators(settings, random);

            engine.EntityMutation = new EntityMutation(settings.SubProblemList, engine.ConflictDetectors, random);
            engine.EntityCrossover = new EntityCrossover(settings.SubProblemList, engine.ConflictDetectors, random);
            engine.StopCriteria = settings.StopCriteria;

            return engine;
        }

        private static ParameterSet CreateParameters(PeaSettings settings)
        {
            var parameterSet = new ParameterSet(settings.ParameterSet);
            var entitiesCount = 1;
            foreach (var subProblem in settings.SubProblemList)
            {
                parameterSet.SetValueRange(subProblem.ParameterSet);
            }
            return parameterSet;
        }

        private static IDictionary<string, IList<IConflictDetector>> CreateConflictDetectors(List<SubProblem> subProblemList)
        {
            var result = new Dictionary<string, IList<IConflictDetector>>();
            foreach (var subProblem in subProblemList)
            {
                string key = subProblem.Encoding.Key;
                var detectors = new List<IConflictDetector>();
                foreach (var detectorType in subProblem.ConflictDetectors)
                {
                    var detectorInstance = (IConflictDetector)Activator.CreateInstance(detectorType);
                    detectors.Add(detectorInstance);
                }
                result.Add(key, detectors);
            }
            return result;
        }

        private static IAlgorithmFactory CreateAlgorithm(IEngine engine, PeaSettings settings)
        {
            //TODO: choose first matching algorithm type from chromosomeFactories, or apply settings
            return new Algorithm.SteadyState();
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

        private static IProvider<IReinsertion> CreateReinsertions(IAlgorithmFactory algorithm, PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var reinsertions = algorithm.GetReinsertions();

            //TODO: impement override by settings


            var reinsertionProvider = CreateProvider<IReinsertion>(reinsertions.Count, random);

            foreach (var reinsertion in reinsertions)
            {
                var reinsertionInstance = (IReinsertion)Activator.CreateInstance(reinsertion, random, fitnessComparer, parameterSet);
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
