namespace Pea.Core
{
    interface IAlgorithm
    {
        IEngine Engine { get; }
        IPopulation Population { get; set; }

        void InitPopulation();
        void RunOnce();
    }
}
