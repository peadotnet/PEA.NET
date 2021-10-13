using Pea.Core;
using Pea.Migration;

namespace Pea.Akka.Messages
{
	public class Travel
    {
        public TravelerTypes TravelerType { get; }
        public IEntityList Members { get; }

        public Travel(IEntityList members, TravelerTypes travelerType)
        {
            TravelerType = travelerType;
            Members = members;
        }
    }
}
