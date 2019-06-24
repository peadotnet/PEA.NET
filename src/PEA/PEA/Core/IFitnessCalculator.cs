namespace Pea.Core
{
    public interface IFitnessCalculator
    {
        void Init(IFitnessCalculatorInitData initData);
        IFitness CalculateFitness(IPhenotype phenotype);
    }
}
