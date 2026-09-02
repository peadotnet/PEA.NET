using Pea.Configuration;
using Pea.Core;
using Pea.Core.Island;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pea
{
    public class Optimizer
    {
        private static readonly ConcurrentDictionary<Guid, Optimizer> _instances = new ConcurrentDictionary<Guid, Optimizer>();

        public Guid Id { get; } = Guid.NewGuid();

        IslandLocalRunner _localRunner = null;

        public PeaSettingsBuilder Settings { get; set; } = new PeaSettingsBuilder();
        //AkkaSystemProvider _provider = new AkkaSystemProvider();


        private Optimizer()
        {
            SetParameter(Core.ParameterNames.PopulationInitTimeout, 60000);
        }

        public static void Reset()
        {
            _instances.Clear();
        }

        public static List<Guid> GetOptimizerIds()
        {
            return new List<Guid>(_instances.Keys);
        }

        public static Optimizer GetOptimizer(Guid id)
        {
            if (_instances.ContainsKey(id)) return null;

            return _instances[id];
        }


        public static void DeleteOptimizer(Guid id)
        {
            _instances.TryRemove(id, out Optimizer removed);
        }

        public static Optimizer Create()
        {
            var optimizer = new Optimizer();
            _instances.TryAdd(optimizer.Id, optimizer);
            return optimizer;
        }

        public Optimizer SetParameter(string parameterKey, double parameterValue)
        {
            Settings.SetParameter(parameterKey, parameterValue);
            return this;
        }

        public async Task<PeaResult> Run(IEvaluationInitData initData) //TODO: Decision based on package
        {
            var settings = Settings.Build();

            var islandsCount = settings.ParameterSet.FindLast(p => p.Name == Core.Island.ParameterNames.IslandsCount)?.Value ?? 1; //TODO: clarify this
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