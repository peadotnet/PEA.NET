using System.Collections.Generic;

namespace Pea.Core
{
    public interface IEvaluation
    {
        void Init(IEvaluationInitData initData);
        IEntity Decode(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);
        IList<IEntity> Combine(MultiKey islandKey, Dictionary<MultiKey, IEntity> entities);
        IEntity AssessFitness(IEntity entity);
    }
}
