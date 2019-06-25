﻿using Pea.Core;
using System;
using System.Reflection;
using Pea.Core.Entity;

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
            foreach (var creator in settings.EntityCreators)
            {
                var creatorInstance = (IEntityCreator)Activator.CreateInstance(creator.ValueType);
                creatorProvider.Add(creatorInstance, 1.0);
            }

            return creatorProvider;
        }

        public static IProvider<ISelection> CreateSelections(PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var selectionProvider = CreateProvider<ISelection>(settings.Selectors.Count, random);
            var selectionParameters = new object[] { random, fitnessComparer, parameterSet };

            foreach (var selectionType in settings.Selectors)
            {
                var selectionInstance = (ISelection)Activator.CreateInstance(selectionType.ValueType, BindingFlags.Default, null, selectionParameters );
                selectionProvider.Add(selectionInstance, 1.0);
            }

            return selectionProvider;
        }

        private static IProvider<IReinsertion> CreateReinsertions(PeaSettings settings, ParameterSet parameterSet, IRandom random, IFitnessComparer fitnessComparer)
        {
            var reinsertionProvider = CreateProvider<IReinsertion>(settings.Reinsertions.Count, random);
            var reinsertionParameters = new object[] { random, fitnessComparer, parameterSet };

            foreach (var reinsertion in settings.Reinsertions)
            {
                var reinsertionInstance = (IReinsertion)Activator.CreateInstance(reinsertion.ValueType, BindingFlags.Default, null, reinsertionParameters);
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
