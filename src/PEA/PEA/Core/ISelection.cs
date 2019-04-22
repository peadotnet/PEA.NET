using System.Collections.Generic;

namespace Pea.Core
{
    public interface ISelection
    {
        IList<IEntity> Select(IList<IEntity> entities);
        IList<IEntity> Select(IList<IEntity> entities, int count);
    }
}
