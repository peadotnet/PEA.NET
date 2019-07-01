using System;

namespace Pea.Core.Entity
{
    public abstract class EntityCreator : IEntityCreator
    {
        public abstract void Init(IEvaluationInitData initData);

        public abstract IEntity CreateEntity();
    }
}
