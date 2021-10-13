using Pea.Core;

namespace Pea.Population.Reduction
{
	public class DoNothingReduction : IReduction
	{
		public IPopulation Reduct(IPopulation population)
		{
			return population;
		}
	}
}
