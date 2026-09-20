using Pea.Core.Entity;
using System.Collections;

namespace Pea.Core
{
    public interface IEntityCreator
    {
        void Init(IEvaluationInitData initData);
        EntityBase CreateEntity();
    }
}
