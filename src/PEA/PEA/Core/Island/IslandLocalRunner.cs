using Pea.Configuration.Implementation;
using Pea.Core.Events;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Pea.Core.Island
{
	public class IslandLocalRunner
	{
		EvaluationBase Evaluator;
		MultiKey Key;
		public IEngine Engine;

		public event NewEntitiesMergedToBestDelegate NewEntitiesMergedToBest;

		public async Task<PeaResult> Run(PeaSettings settings, IEvaluationInitData initData, LaunchTravelersDelegate launchTravelers = null)
		{
			return await Task.Run(() =>
			{
				string[] keys = new string[settings.SubProblemList.Count];
				for (int i = 0; i < settings.SubProblemList.Count; i++)
				{
					keys[i] = settings.SubProblemList[i].Encoding.Key;
				}
				Key = new MultiKey(keys);

				Engine = IslandEngineFactory.Create(Key, settings, settings.Seed);

				AddCallbackEvents(Engine, settings.NewEntityMergedToBest);
				if (launchTravelers != null) Engine.LaunchTravelers += launchTravelers;

				Evaluator = (EvaluationBase)TypeLoader.CreateInstance(settings.Evaluation, Engine.Parameters);
				Evaluator.Init(initData);

				Engine.Algorithm.SetEvaluationCallback(Evaluate);

				Engine.Init(initData);

				if (Engine.Algorithm.Population.Count == 0)
				{
					var reasons = new List<string>() { "Initialization of population timed out." };
					return new PeaResult(reasons, Engine.Algorithm.Population.Bests);
				}

				var c = 0;
				StopDecision stopDecision;
				while (true)
				{
					stopDecision = Engine.RunOnce();
					if (stopDecision.MustStop)
					{
						Debug.WriteLine(stopDecision.Reasons[0]);
						break;
					}
					c++;
				}

				return new PeaResult(stopDecision.Reasons, Engine.Algorithm.Population.Bests);
			});
		}

		private void AddCallbackEvents(IEngine engine, List<NewEntitiesMergedToBestDelegate> delegates)
		{
			if (delegates.Count > 0)
			{
				for (int d = 0; d < delegates.Count; d++)
				{
					engine.NewEntityMergedToBest += delegates[d];
				}
			}
		}

		public IEntityList Evaluate(IEntityList entityList)
		{
			if (entityList.Count == 0) return entityList;

			var evaluatedEntities = new EntityList(entityList.Count);

			for (int i = 0; i < entityList.Count; i++)
			{
				var entityWithKey = new Dictionary<MultiKey, IEntity> { { Key, entityList[i] } };
				var decodedEntity = Evaluator.Decode(Key, entityWithKey);
				if (decodedEntity != null) evaluatedEntities.Add(decodedEntity);
			}

			return evaluatedEntities;
		}

		public void SetBestMergedDelegate(LaunchTravelersDelegate mergedDelegate)
		{
		}

		public IEvolutionStateReportData GetCurrentState()
		{
			return Engine?.GetCurrentState();
		}
	}
}
