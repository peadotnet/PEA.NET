using System.Collections.Generic;

namespace Pea.Core
{
    public delegate IList<IEntity> EvaluationDelegate(IList<IEntity> entityList);

    public interface IAlgorithm
    {
        IEngine Engine { get; }
        IPopulation Population { get; set; }

        void SetEvaluationCallback(EvaluationDelegate evaluationCallback);
        void InitPopulation();
        void RunOnce();
    }
}
