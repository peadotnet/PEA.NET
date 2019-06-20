using System;
using System.Collections.Generic;
using Pea.Core;

namespace Pea.Selection
{
    public class TournamentSelection : SelectionBase
    {
        public TournamentSelection(IRandom random, IFitnessComparer fitnessComparer, IParameterSet parameterSet)
            : base(random, fitnessComparer, parameterSet)
        {
        }

        public override IList<IEntity> Select(IList<IEntity> entities)
        {
            IList<IEntity> result = new List<IEntity>();
            var size = Convert.ToInt32(ParameterSet.GetValue(ParameterNames.TournamentSize));
            var first = SelectOne(entities, size);
            result.Add(first);
            var second = first;
            while (second == first)
            {
                second = SelectOne(entities, size);
            }

            result.Add(second);
            return result;
        }

        private IEntity SelectOne(IList<IEntity> entities, int size)
        {
            var index = Random.GetInt(0, entities.Count);
            var best = entities[index];

            for (int i = 1; i < size; i++)
            {
                index = Random.GetInt(0, entities.Count);
                var next = entities[index];
                var comparisonResult = FitnessComparer.Compare(best.Fitness, next.Fitness);
                if (comparisonResult > 0)
                {
                    best = next;
                }
            }

            return best;
        }
    }
}
