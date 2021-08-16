using Pea.Configuration.Implementation;
using System.Collections.Generic;

namespace Pea.Core
{
	public abstract class EvaluationBase
	{
		public ParameterSet Parameters { get; set; }

		public EvaluationBase(ParameterSet parameters)
		{
			Parameters = parameters;
		}

		public abstract void Init(IEvaluationInitData initData);

		public abstract IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);

		public abstract IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);
	}
}
