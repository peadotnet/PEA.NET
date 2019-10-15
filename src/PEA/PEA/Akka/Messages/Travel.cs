using System.Collections.Generic;
using Pea.Core;
using Pea.Migration;

namespace Pea.Akka.Messages
{
    public class Travel
    {
        public TravelerTypes TravelerType { get; }
        public IList<IEntity> Members { get; }

        public Travel(IList<IEntity> members, TravelerTypes travelerType)
        {
            TravelerType = travelerType;
            Members = members;
        }
    }
}
