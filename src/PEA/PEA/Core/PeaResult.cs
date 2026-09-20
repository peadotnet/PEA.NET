using Pea.Core.Entity;
using System.Collections.Generic;

namespace Pea.Core
{
    public class PeaResult
    {
        public IList<string> StopReasons { get; }
        public IList<EntityBase> BestSolutions { get; }

        public PeaResult(IList<string> stopReasons, IList<EntityBase> bestSolutions)
        {
            StopReasons = stopReasons;
            BestSolutions = bestSolutions;
        }
    }
}
