using System.Collections.Generic;

namespace Pea.Core
{
    public interface IEntityCrossover
    {
        IList<IEntity> Cross(IList<IEntity> parents, int count);
    }
}
