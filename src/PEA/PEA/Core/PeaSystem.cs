using Pea.Core.Settings;

namespace Pea.Core
{
    public class PeaSystem
    {
        public PeaSettings Settings { get; set; } = new PeaSettings();

        private PeaSystem()
        {
            
        }

        public static PeaSystem Create()
        {
            return new PeaSystem();
        }

        public PeaSystem WithAlgorithm<TA>() where TA : IAlgorithmFactory
        {
            Settings.Algorithm = typeof(TA);
            return this;
        }

        public PeaSystem AddChromosome<TCH>(string name) where TCH : IChromosomeFactory
        {
            Settings.Chromosomes.Add(new PeaSettingsNamedType(name, typeof(TCH)));
            return this;
        }

        public PeaSystem WithCreator<TEC>(params string[] chromosomeNames) where TEC : IEntityCreator
        {
            Settings.EntityCreators.Add(new PeaSettingsNamedType(chromosomeNames, typeof(TEC)));
            return this;
        }

        public PeaSystem AddSelection<TS>(double probability = 1.0) where TS: ISelection
        {
            Settings.Selectors.Add(new PeaSettingsTypeProbability(typeof(TS), probability));
            return this;
        }

        public PeaSystem AddReinsertion<TR>(double probability = 1.0)
        {
            Settings.Reinsertions.Add(new PeaSettingsTypeProbability(typeof(TR), probability));
            return this;
        }

        public PeaSystem WithPhenotypeDecoder<TPD>() where TPD: IPhenotypeDecoder
        {
            Settings.PhenotypeDecoder = typeof(TPD);
            return this;
        }

        public PeaSystem WithFitnessEvaluator<TFE>() where TFE : IFitnessEvaluator
        {
            Settings.FitnessEvaluator = typeof(TFE);
            return this;
        }

        public PeaSystem WithFitness<TF>() where TF: IFitnessFactory
        {
            Settings.Fitness = typeof(TF);
            return this;
        }

        public PeaSystem SetParameter(string name, double value)
        {
            Settings.ParameterSet.Add(new PeaSettingsNamedValue(name, value));
            return this;
        }

        public void Start()
        {

        }
    }
}
