using System.Collections.Generic;

namespace Pea.Core
{
    public interface IReplacement
    {
        IList<IEntity> Replace(IList<IEntity> target, IList<IEntity> entities);
    }
}
