using System.Collections.Generic;

namespace Pea.Core
{
    public interface IEntityMutation
    {
        IList<IEntity> Mutate(IList<IEntity> entities);
    }
}
