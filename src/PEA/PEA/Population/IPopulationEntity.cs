using Pea.Core;

namespace Pea.Population
{
	public interface IPopulationEntity : IEntity
	{
		int IndexInPopulation { get; set; }
	}
}
