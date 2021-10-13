using System;
using System.Collections.Generic;
using Pea.Core;
using Pea.Population;

namespace Pea.Selection
{
    public class TournamentSelection : SelectionBase
    {
        public TournamentSelection(IRandom random, IFitnessComparer fitnessComparer, IParameterSet parameterSet)
            : base(random, fitnessComparer, parameterSet)
        {
        }

        public override IEntityList Select(IEntityList entities, int count)
		{
            if (count == 1)
            {
                var tournamentSize = Convert.ToInt32(ParameterSet.GetValue(ParameterNames.TournamentSize));
                var selected = SelectOne(entities, tournamentSize);
                var entityList = new EntityList(1);
                entityList.Add(selected);
                return entityList;
            }
            //else if (count < 20)
            //{
                return SelectWithList(entities, count);
            //}
            //else
            //{
            //    return SelectWithHashSet(entities, count);
            //}
		}

        IEntityList SelectWithList(IEntityList entities, int count)
        {
            List<IEntity> result = new List<IEntity>(count);
            var tournamentSize = Convert.ToInt32(ParameterSet.GetValue(ParameterNames.TournamentSize));

            for (int i = 0; i < count; i++)
            {
                var selected = SelectOne(entities, tournamentSize);
    //            while(result.Contains(selected))
				//{
                    //selected = SelectOne(entities, tournamentSize);
                //}
                result.Add(selected);
            }
            return new EntityList(result);
        }

        IEntityList SelectWithHashSet(IEntityList entities, int count)
        {
            HashSet<IEntity> result = new HashSet<IEntity>();
            var tournamentSize = Convert.ToInt32(ParameterSet.GetValue(ParameterNames.TournamentSize));

            for(int i=0; i< count; i++)
            {
                var selected = SelectOne(entities, tournamentSize);
                while (result.Contains(selected))
                {
                    selected = SelectOne(entities, tournamentSize);
                }
                result.Add(selected);
            }
            return new EntityList(result);
        }

        private IEntity SelectOne(IEntityList entities, int tournamentSize)
        {
            var index = Random.GetInt(0, entities.Count);
            var best = entities[index];

            for (int i = 1; i < tournamentSize; i++)
            {
                index = Random.GetInt(0, entities.Count);
                var next = entities[index];
                var comparisonResult = FitnessComparer.Compare(best.Fitness, next.Fitness);
                if (comparisonResult > 0)
                {
                    next.Fitness.TournamentWinner++;
                    best.Fitness.TournamentLoser++;
                    best = next;
                }
                else
				{
                    best.Fitness.TournamentWinner++;
                    next.Fitness.TournamentLoser++;
				}
            }

            return best;
        }
    }
}
