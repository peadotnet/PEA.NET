using System;

namespace Pea.Core
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
