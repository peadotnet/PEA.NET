using Pea.Evaluation;
using System.Collections;

namespace Pea.Entity
{
    public interface IEntityCreator
    {
        void Init(IEvaluationInitData initData);
        EntityBase CreateEntity();
    }
}
