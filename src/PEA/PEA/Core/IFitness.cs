namespace Pea.Core
{
    public interface IFitness
    {
        bool IsValid { get; set; }
    }

    public interface IFitness<TF> : IFitness
    {
        TF Value { get; set; }
    }
}
