namespace Pea.Core
{
    public interface IEngine
    {
        IProvider<IEntityCreator> EntityCreators { get; set; }
        IEntityCrossover EntityCrossover { get; set; }
        IEntityMutation EntityMutation { get; set; }
        IFitnessCalculator FitnessCalculator { get; set; }
        IFitnessComparer FitnessComparer { get; set; }
        IPhenotypeDecoder PhenotypeDecoder { get; set; }
        IProvider<IReplacement> Replacements { get; set; }
        IProvider<ISelection> Selectors { get; set; }

        void Init();
        bool RunOnce();
    }
}