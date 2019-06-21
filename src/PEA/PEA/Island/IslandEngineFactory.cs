using Pea.Core;
using System;
using System.Reflection;

namespace Pea.Island
{
    public static class IslandEngineFactory
    {
        public static IslandEngine Create(PeaSettings settings)
        {
            var engine = new IslandEngine();

            var random = (IRandom)Activator.CreateInstance(settings.Random);
            var parameterSet = new ParameterSet(settings.ParameterSet);
            var fitness = (IFitnessFactory)Activator.CreateInstance(settings.Fitness);
            var fitnessComparer = fitness.GetFitnessComparer();

            engine.FitnessComparer = fitnessComparer;
            //TODO: engine.EntityCreators = CreateEntityCreators(settings, random);
            //TODO: engine.PhenotypeDecoder
            //TODO: engine.FitnessEvaluator
            engine.Selections = CreateSelections(settings, parameterSet, random, fitnessComparer);
            engine.Reinsertions = CreateReinsertions(settings, parameterSet, random, fitnessComparer);
            engine.EntityMutation = new EntityMutation(settings.Chromosomes, random);
            engine.EntityCrossover = new EntityCrossover(settings.Chromosomes, random);

            //TODO: engine.StopCriteria json builder

            return engine;
        }

        public static IProvider<IEntityCreator> CreateEntityCreators(PeaSettings settings, IRandom random)
        {
            var creatorProvider = CreateProvider<IEntityCreator>(settings.EntityCreators.Count, random);
            foreach (var creatorType in settings.EntityCreators)
            {
                var creator = (IEntityCreator)Activator.CreateInstance(creatorType.Key);
                creatorProvider.Add(creator, creatorType.Value);
            }

            return creatorProvider;
        }

        public static IProvider<ISelection> CreateSelections(PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var selectionProvider = CreateProvider<ISelection>(settings.Selectors.Count, random);
            var selectionParameters = new object[] { random, fitnessComparer, parameterSet };

            foreach (var selectionType in settings.Selectors)
            {
                var selection = (ISelection)Activator.CreateInstance(selectionType.Key, BindingFlags.Default, null, selectionParameters );
                selectionProvider.Add(selection, selectionType.Value);
            }

            return selectionProvider;
        }

        private static IProvider<IReinsertion> CreateReinsertions(PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var reinsertionProvider = CreateProvider<IReinsertion>(settings.Reinsertions.Count, random);
            var reinsertionParameters = new object[] { random, fitnessComparer, parameterSet };

            foreach (var reinsertionType in settings.Reinsertions)
            {
                var reinsertion = (IReinsertion)Activator.CreateInstance(reinsertionType.Key, BindingFlags.Default, null, reinsertionParameters);
                reinsertionProvider.Add(reinsertion, reinsertionType.Value);
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
