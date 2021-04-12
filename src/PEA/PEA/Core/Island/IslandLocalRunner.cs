using Pea.Akka.Messages;
using Pea.Configuration.Implementation;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pea.Core.Island
{
	public class IslandLocalRunner
	{
		IEvaluation Evaluator;
		MultiKey Key;

		public PeaResult Run(PeaSettings settings, IEvaluationInitData initData, LaunchTravelersDelegate launchTravelers = null)
		{
			string[] keys = new string[settings.SubProblemList.Count];
			for(int i=0; i< settings.SubProblemList.Count; i++)
			{
				keys[i] = settings.SubProblemList[i].Encoding.Key;
			}
			Key = new MultiKey(keys);

			var islandEngine = IslandEngineFactory.Create(Key, settings);

			Evaluator = (IEvaluation)TypeLoader.CreateInstance(settings.Evaluation);
			Evaluator.Init(initData);

			var algorithmFactory = new Pea.Algorithm.SteadyState();
			var algorithm = algorithmFactory.GetAlgorithm(islandEngine);
			algorithm.SetEvaluationCallback(Evaluate);
			islandEngine.Algorithm = algorithm;
			islandEngine.Init(initData);
			if (launchTravelers != null) islandEngine.LaunchTravelers += launchTravelers;

			var c = 0;
			StopDecision stopDecision;
			while (true)
			{
				algorithm.RunOnce();
				stopDecision = islandEngine.StopCriteria.MakeDecision(islandEngine, algorithm.Population);
				if (stopDecision.MustStop)
				{
					Debug.WriteLine(stopDecision.Reasons[0]);
					break;
				}
				c++;
			}

			return new PeaResult(stopDecision.Reasons, algorithm.Population.Bests);
		}

		public IList<IEntity> Evaluate(IList<IEntity> entityList)
		{
			if (entityList.Count == 0) return entityList;

			IList<IEntity> evaluatedEntities = new List<IEntity>();

			for (int i=0; i< entityList.Count; i++)
			{
				var entityWithKey = new Dictionary<MultiKey, IEntity> { { Key, entityList[i] } };
				var decodedEntity = Evaluator.Decode(Key, entityWithKey);
				evaluatedEntities.Add(decodedEntity);
			}

			return evaluatedEntities;
		}

		public void SetBestMergedDelegate(LaunchTravelersDelegate mergedDelegate)
		{
		}
	}
}
