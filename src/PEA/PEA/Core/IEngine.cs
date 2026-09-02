using System.Collections.Generic;
using Pea.Core.Events;
using Pea.Migration;

namespace Pea.Core
{
    public delegate void LaunchTravelersDelegate(IEntityList entityList, TravelerTypes travelerType);

    public interface IEngine
    {
        int Iteration { get; set; }
        
        LaunchTravelersDelegate LaunchTravelers { get; set; }

        event NewEntitiesMergedToBestDelegate NewEntityMergedToBest;

        IAlgorithm Algorithm { get; set; }
        IRandom Random { get; }
        IReduction Reduction { get; set; }
        EvaluationBase Evaluation { get; set; }
        ParameterSet Parameters { get; }

        void Init(IEvaluationInitData initData);
        StopDecision RunOnce();
        void MergeToBests(IEntityList entities);
        void Reduct();
        void TravelersArrived(IEntityList travelers);

        IEvolutionStateReportData GetCurrentState();
    }
}