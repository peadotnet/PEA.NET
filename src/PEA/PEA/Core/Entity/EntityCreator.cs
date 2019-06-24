using System;

namespace Pea.Core.Entity
{
    public class EntityCreator<TG> : IEntityCreator<TG>
        where TG : IGenotype, new()
    {
        public IEntity CreateEntity()
        {
            throw new NotImplementedException();
        }
    }
}
