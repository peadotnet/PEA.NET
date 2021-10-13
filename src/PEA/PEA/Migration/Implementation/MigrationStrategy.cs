using Pea.Core;

namespace Pea.Migration.Implementation
{
	public class MigrationStrategy : MigrationStrategyBase
    {
        public IRandom Random { get; }

        public ISelection Selection { get; }

        public IReplacement Reinsertion { get; }

        public ParameterSet Parameters { get; }

        public MigrationStrategy(IRandom random, ISelection selection, IReplacement reinsertion, ParameterSet parameters) : base(random, selection, reinsertion, parameters)
        {
            Random = random;
            Selection = selection;
            Reinsertion = reinsertion;
            Parameters = parameters;
        }

        public override IEntityList SelectForTraveling(IPopulation population)
        {
            var travelers = new EntityList(4);

            var migrationFrequency = Parameters.GetValue(ParameterNames.MigrationFrequency);
            var mustLaunch = (Random.GetDouble(0, 1) < migrationFrequency);

            if (mustLaunch)
            {
                var count = Parameters.GetValue(ParameterNames.MigrationCount);

                for (int i = 0; i < count; i++)
                {
                    var traveler = Selection.Select(population, 1);
                    travelers.Add(traveler[0]);
                }
            }

            return travelers;
        }

        public override bool TravelerReceptionDecision(IPopulation population)
        {
            var receptionRate = Parameters.GetValue(ParameterNames.MigrationReceptionRate);
            var receptionDecision = Random.GetDouble(0, 1) < receptionRate;
            return receptionDecision;
        }

        public override IEntityList InsertMigrants(IPopulation population, IEntityList travelers)
        {
            Reinsertion.Replace(population, travelers, null, population);
            return population;
        }
    }
}
