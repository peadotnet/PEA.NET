namespace Pea.Fitness
{
    public interface IFitnessFactory
    {
        IFitness GetFitness();
        IFitnessComparer GetFitnessComparer();
    }
}
