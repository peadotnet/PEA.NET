using Pea.Core;

namespace PEA_VehicleScheduling_Example
{
    public class VSConflictDetector : INeighborhoodConflictDetector
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

            var earliestArrivalTime = trip1.DepartureTime + duration;
            var route1 = trip1.TripId.Substring(0, trip1.TripId.IndexOf('_'));
            var route2 = trip2.TripId.Substring(0, trip2.TripId.IndexOf('_'));
            if (route1 != route2)
            {
                earliestArrivalTime += 2;
            }

            if (earliestArrivalTime > trip2.ArrivalTime)
            {
                return true;
            }

            return false;
        }

		public bool ConflictDetected(IEntity entity, int first, int second)
		{
            return ConflictDetected(first, second);
		}
	}
}
