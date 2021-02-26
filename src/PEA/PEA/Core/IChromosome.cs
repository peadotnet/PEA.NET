namespace Pea.Core
{
    public interface IChromosome : IDeepCloneable<IChromosome>
    {
        IEntity Entity { get; set; }
    }
}
