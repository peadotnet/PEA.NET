namespace Pea.Chromosome
{
    public interface INeighborhoodCostDetector
    {
        double GetCost(int first, int second);
    }
}
