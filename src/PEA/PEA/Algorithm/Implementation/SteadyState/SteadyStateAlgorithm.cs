﻿using Pea.Core;

namespace Pea.Algorithm.Implementation
{
    public class SteadyStateAlgorithm : GeneticAlgorithmBase
    {
        public SteadyStateAlgorithm(IEngine engine) : base(engine)
        {
        }

        public override void InitPopulation()
        {
            var fitnessLength = Engine.Parameters.GetInt(ParameterNames.FitnessLength);
            var maxNumberOfEntities = Engine.Parameters.GetInt(ParameterNames.PopulationSize);
            var minNumberOfEntities = System.Convert.ToInt32(Engine.Parameters.GetValue(ParameterNames.SelectionRate) * maxNumberOfEntities);

            IEntityList initialEntities = new EntityList(maxNumberOfEntities);
            for (int i = 0; i < maxNumberOfEntities; i++)
            {
                var entity = CreateEntity();
                initialEntities.Add(entity);
            }

            initialEntities = Evaluate(initialEntities);

            Population = new Population.Population(fitnessLength, minNumberOfEntities, maxNumberOfEntities);

            for (int i = 0; i < maxNumberOfEntities; i++)
            {
                Population.Add(initialEntities[i]);
            }

            MergeToBests(Population);
        }

        public override StopDecision RunOnce()
        {
            var parents = SelectParents(Population, 2);
            var offspring = Crossover(parents, 2);
            var mutated = Mutate(offspring);
            var evaluated = Evaluate(mutated);
            var inserted = Reinsert(Population, evaluated, parents, Population);
            MergeToBests(inserted);
            return StopCriteria.MakeDecision(this.Engine, this.Population);

        }
    }
}
