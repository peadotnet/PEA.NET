using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Core
{
    public interface INiching
    {
        IList<EntityBase> Niching(IList<EntityBase> entities);
    }
}
