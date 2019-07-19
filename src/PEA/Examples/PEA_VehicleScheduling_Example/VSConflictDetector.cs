using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VSConflictDetector : IConflictDetector
    {
        public VSInitData InitData { get; private set; }

        public void Init(IEvaluationInitData initData)
        {
            InitData = (VSInitData)initData;
            InitData.Build();
        }

        public bool ConflictDetected(int first, int second)
        {
            var trip1 = InitData.Trips[first];
            var trip2 = InitData.Trips[second];

            double duration = InitData.GetDuration(trip1.LastStopId, trip2.FirstStopId);

            if (trip1.DepartureTime + duration > trip2.ArrivalTime)
            {
                return true;
            }

            return false;
        }
    }
}
