using System.Collections.Generic;

namespace Pea.Core
{
    public interface ICrossover<TG>
    {
        IList<IEntity<TG>> Cross(IList<IEntity<TG>> parents);
    }
}
