using System.Collections.Generic;
using Pea.Core;

namespace PEA_TSP_Example
{
    public class TSPInitData : IEvaluationInitData
    {
        public List<SpatialPoint> TSPPoints { get; }

        public TSPInitData(List<SpatialPoint> tspPoints)
        {
            TSPPoints = tspPoints;
        }

        public void Build()
        {
        }
    }
}
