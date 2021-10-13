namespace Pea.Core
{
	public interface IReplacement
    {
        IEntityList Replace(IPopulation targetPopulation, IEntityList offspring, IEntityList parents, IPopulation sourcePopulation);
    }
}