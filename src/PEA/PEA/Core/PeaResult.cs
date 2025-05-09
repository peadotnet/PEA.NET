using System.Collections.Generic;

namespace Pea.Core
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
