using System.Collections.Generic;

namespace Pea.Population
{
	public interface IReduction
	{
		IPopulation Reduct(IPopulation population);
	}
}
