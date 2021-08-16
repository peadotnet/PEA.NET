using Pea.Configuration.Implementation;
using System.Collections.Generic;

namespace Pea.Core
{
	public abstract class EvaluationBase
	{
		ParameterSet ParameterSet { get; set; }

		public EvaluationBase(ParameterSet parameterSet)
		{
			ParameterSet = parameterSet;
		}

		public abstract void Init(IEvaluationInitData initData);

		public abstract IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);

		public abstract IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);
	}
}
