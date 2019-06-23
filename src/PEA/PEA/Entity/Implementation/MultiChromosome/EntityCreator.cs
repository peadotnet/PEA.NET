using Pea.Core;
using System;

namespace Pea.Entity.Implementation.MultiChromosome
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
