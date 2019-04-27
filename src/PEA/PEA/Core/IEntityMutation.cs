using System;
using System.Collections.Generic;
using System.Text;

namespace Pea.Core
{
    public interface IEntityMutation
    {
        IList<IEntity> Mutate(IList<IEntity> entities);
    }
}
