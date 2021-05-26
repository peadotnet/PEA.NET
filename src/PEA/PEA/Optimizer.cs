using Pea.Akka;
using Pea.Akka.Messages;
using Pea.Configuration;
using Pea.Core;
using Pea.Core.Island;

namespace Pea
{
	public class Optimizer
    {
        public PeaSettingsBuilder Settings { get; set; } = new PeaSettingsBuilder();
        AkkaSystemProvider _provider = new AkkaSystemProvider();


        private Optimizer()
        {

        }

        public static Optimizer Create()
        {
            return new Optimizer();
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

        public Optimizer SetParameter(string parameterKey, double parameterValue)
        {
            Settings.SetParameter(parameterKey, parameterValue);
            return this;
        }



        public PeaResult Run(IEvaluationInitData initData) //async Task<PeaResult>
        {
            var settings = Settings.Build();

            var islandsCount = settings.ParameterSet.FindLast(p => p.Name == ParameterNames.IslandsCount).Value; //TODO: clarify this
            PeaResult result = null;
			if (islandsCount < 2)
			{
				var localRunner = new IslandLocalRunner();
				result = localRunner.Run(settings, initData);
			}
			else
			{
				result = _provider.Start(settings, initData);  //await
			}
			return result;
        }
    }
}
