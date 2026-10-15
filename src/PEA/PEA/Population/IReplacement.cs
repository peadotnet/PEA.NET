using Pea.Entity;

namespace Pea.Population
{
	public interface IReplacement
    {
        IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation);
    }
}