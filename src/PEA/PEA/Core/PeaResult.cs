using System.Collections.Generic;
using Pea.Core;

namespace Pea.Akka.Messages
{
    public class PeaResult
    {
        public IList<string> StopReasons { get; }
        public IList<IEntity> BestSolutions { get; }

        public PeaResult(IList<string> stopReasons, IList<IEntity> bestSolutions)
        {
            StopReasons = stopReasons;
            BestSolutions = bestSolutions;
        }
    }
}
