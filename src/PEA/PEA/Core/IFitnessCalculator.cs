namespace Pea.Core
{
    public interface IFitnessCalculator
    {
        IFitness CalculateFitness(IPhenotype phenotype);
    }
}
