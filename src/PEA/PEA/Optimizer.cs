using Pea.Configuration;
using Pea.Core;
using Pea.Core.Island;
using System.Threading.Tasks;

namespace Pea
{
    public class Optimizer
    {
        private static Optimizer _instance = null;
        IslandLocalRunner _localRunner = null;

        public PeaSettingsBuilder Settings { get; set; } = new PeaSettingsBuilder();
        //AkkaSystemProvider _provider = new AkkaSystemProvider();


        private Optimizer()
        {
            SetParameter(Core.ParameterNames.PopulationInitTimeout, 60000);
        }

        public static void Reset()
        {
            _instance = null;
        }

        public static Optimizer Create()
        {
            if (_instance == null) _instance = new Optimizer();
            return _instance;
        }

        public Optimizer SetParameter(string parameterKey, double parameterValue)
        {
            Settings.SetParameter(parameterKey, parameterValue);
            return this;
        }

        public async Task<PeaResult> Run(IEvaluationInitData initData) //async Task<PeaResult>
        {
            var settings = Settings.Build();

            var islandsCount = settings.ParameterSet.FindLast(p => p.Name == Core.Island.ParameterNames.IslandsCount).Value; //TODO: clarify this
            PeaResult result = null;
            if (islandsCount < 2)
            {
                _localRunner = new IslandLocalRunner();
                result = await _localRunner.Run(settings, initData);
            }
            else
            {
                //result = _provider.Start(settings, initData);  //await
            }
            return result;
        }

        public IEvolutionStateReportData GetCurrentState()
        {
            return _localRunner?.GetCurrentState();
        }
    }
}