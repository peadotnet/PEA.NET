using System.Collections.Generic;

namespace Pea.Core
{
	public interface IReduction
	{
		IPopulation Reduct(IPopulation population);
	}
}
