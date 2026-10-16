using System.Collections.Generic;
using Pea.Algorithm;
using Pea.Entity;
using Pea.Evaluation;
using Pea.Migration;
using Pea.Population;
using Pea.Rnd;
using Pea.StopCriteria;

namespace Pea.Engine
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