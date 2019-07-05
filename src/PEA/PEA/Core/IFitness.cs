namespace Pea.Core
{
    public interface IFitness
    {
        bool IsValid { get; set; }
        bool IsEquivalent(IFitness other);
    }

    public interface IFitness<TF> : IFitness
    {
        TF Value { get; set; }
    }
}
