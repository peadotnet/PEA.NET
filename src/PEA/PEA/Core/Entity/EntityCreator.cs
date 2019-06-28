using System;

namespace Pea.Core.Entity
{
    public abstract class EntityCreator : IEntityCreator
    {
        public abstract IEntity CreateEntity();
    }
}
