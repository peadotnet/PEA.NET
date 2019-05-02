namespace Pea.Core
{
    public interface IFitnessFactory
    {
        IFitness GetFitness();
        IFitnessComparer GetFitnessComparer();
    }
}
