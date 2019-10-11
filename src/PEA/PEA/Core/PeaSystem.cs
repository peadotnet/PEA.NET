using System.Threading.Tasks;
using Pea.Akka;
using Pea.Akka.Messages;
using Pea.Configuration;
using Pea.Configuration.Implementation;

namespace Pea.Core
{
    public class PeaSystem
    {
        public PeaSettingsBuilder Settings { get; set; } = new PeaSettingsBuilder();

        private PeaSystem()
        {
            
        }

        public static PeaSystem Create()
        {
            return new PeaSystem();
        }

        //public PeaSystem WithAlgorithm<TA>() where TA : IAlgorithmFactory
        //{
        //    Settings.Algorithm = typeof(TA);
        //    return this;
        //}

        //public PeaSystem AddChromosome<TCH>(string name) where TCH : IChromosomeFactory
        //{
        //    Settings.Chromosomes.Add(new PeaSettingsNamedType(name, typeof(TCH)));
        //    return this;
        //}

        //public PeaSystem WithConflictDetector<CD>() where CD : IConflictDetector
        //{
        //    Settings.ConflictDetector = typeof(CD);
        //    return this;
        //}

        //public PeaSystem WithCreator<TEC>(params string[] chromosomeNames) where TEC : IEntityCreator
        //{
        //    var multiKey = new MultiKey(chromosomeNames);
        //    Settings.EntityCreators.Add(new PeaSettingsNamedType(multiKey, typeof(TEC)));
        //    return this;
        //}

        //public PeaSystem AddSelection<TS>(double probability = 1.0) where TS: ISelection
        //{
        //    Settings.Selectors.Add(new PeaSettingsTypeProbability(typeof(TS), probability));
        //    return this;
        //}

        //public PeaSystem AddReinsertion<TR>(double probability = 1.0)
        //{
        //    Settings.Reinsertions.Add(new PeaSettingsTypeProbability(typeof(TR), probability));
        //    return this;
        //}

        //public PeaSystem WithEvaluation<TFE>() where TFE : IEvaluation
        //{
        //    Settings.Evaluation = typeof(TFE);
        //    return this;
        //}

        //public PeaSystem WithFitness<TF>() where TF: IFitnessFactory
        //{
        //    Settings.Fitness = typeof(TF);
        //    return this;
        //}

        public PeaSystem SetParameter(string parameterKey, double parameterValue)
        {
            Settings.SetParameter(parameterKey, parameterValue);
            return this;
        }

        public PeaResult Start(IEvaluationInitData initData) //async Task<PeaResult>
        {
            AkkaSystemProvider provider = new AkkaSystemProvider();
            PeaResult result = provider.Start(Settings.Build(), initData);  //await
            return result;
        }
    }
}
