namespace Pea.Core
{
    interface IAlgorithm
    {
        StochasticProvider<IEntityCreator> EntityCreators { get; set; }
        StochasticProvider<ISelection> Selectors { get; set; }
        StochasticProvider<IReplacement> Replacements { get; set; }
        IPhenotypeDecoder fenotypeDecoder { get; set; }
        IFitnessCalculator fitnessCalculator { get; set; }
        IPopulation Population { get; set; }
        IEntityCrossover EntityCrossover { get; set; }
        IEntityMutation EntityMutation { get; set; }

        void InitPopulation();
        void RunOnce();
    }
}
