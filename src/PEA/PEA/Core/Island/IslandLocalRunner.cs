using Pea.Akka.Messages;
using Pea.Configuration.Implementation;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pea.Core.Island
{
	public class IslandLocalRunner
	{
		EvaluationBase Evaluator;
		MultiKey Key;

		event NewEntitiesMergedToBestDelegate NewEntitiesMergedToBest;

		public PeaResult Run(PeaSettings settings, IEvaluationInitData initData, LaunchTravelersDelegate launchTravelers = null)
		{
			string[] keys = new string[settings.SubProblemList.Count];
			for(int i=0; i< settings.SubProblemList.Count; i++)
			{
				keys[i] = settings.SubProblemList[i].Encoding.Key;
			}
			Key = new MultiKey(keys);

			var islandEngine = IslandEngineFactory.Create(Key, settings, settings.Seed);

			AddCallbackEvents(islandEngine, settings.NewEntityMergedToBest);

			Evaluator = (EvaluationBase)TypeLoader.CreateInstance(settings.Evaluation, islandEngine.Parameters);
			Evaluator.Init(initData);

			islandEngine.Algorithm.SetEvaluationCallback(Evaluate);
			islandEngine.Init(initData);
			if (launchTravelers != null) islandEngine.LaunchTravelers += launchTravelers;

			var c = 0;
			StopDecision stopDecision;
			while (true)
			{
				islandEngine.Algorithm.RunOnce();
				stopDecision = islandEngine.StopCriteria.MakeDecision(islandEngine, islandEngine.Algorithm.Population);
				if (stopDecision.MustStop)
				{
					Debug.WriteLine(stopDecision.Reasons[0]);
					break;
				}
				c++;
			}

			return new PeaResult(stopDecision.Reasons, islandEngine.Algorithm.Population.Bests);
		}

		private void AddCallbackEvents(IslandEngine engine, List<NewEntitiesMergedToBestDelegate> delegates)
		{
			if (delegates.Count > 0)
			{
				for (int d = 0; d < delegates.Count; d++)
				{
					NewEntitiesMergedToBest += delegates[d];
				}
				engine.NewEntityMergedToBest = NewEntitiesMergetToBestCallback;
			}
		}

		private void NewEntitiesMergetToBestCallback(IList<IEntity> bests)
		{
			if (NewEntitiesMergedToBest != null) NewEntitiesMergedToBest(bests);
		}


		public IEntityList Evaluate(IEntityList entityList)
		{
			if (entityList.Count == 0) return entityList;

			var evaluatedEntities = new EntityList(entityList.Count);

			for (int i=0; i< entityList.Count; i++)
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
	}
}
