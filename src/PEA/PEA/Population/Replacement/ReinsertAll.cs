using Pea.Core;

namespace Pea.Population.Replacement
{
	public class ReinsertAll : ReplacementBase
	{
		public ReinsertAll(IRandom random, IFitnessComparer fitnessComparer, ParameterSet parameters) : base(random, fitnessComparer, parameters)
		{
		}

		public override IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation)
		{
			AddEntitiesToPopulation(targetPopulation, offspring);
			return offspring;
		}
	}
}
