using System.Collections.Generic;

namespace Pea.Core
{
    public interface INiching
    {
        IList<IEntity> Niching(IList<IEntity> entities);
    }
}
