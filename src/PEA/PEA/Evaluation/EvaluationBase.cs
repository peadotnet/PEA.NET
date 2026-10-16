using Pea.Entity;
using System.Collections.Generic;

namespace Pea.Evaluation
{
	public abstract class EvaluationBase
	{
		public ParameterSet Parameters { get; set; }

		public EvaluationBase(ParameterSet parameters)
		{
			Parameters = parameters;
		}

		public abstract void Init(IEvaluationInitData initData);

		public abstract EntityBase Decode(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities);

		public abstract IList<EntityBase> Combine(MultiKey islandKey, Dictionary<MultiKey, EntityBase> entities);
	}
}
