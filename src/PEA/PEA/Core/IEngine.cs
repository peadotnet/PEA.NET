using System.Collections.Generic;
using Pea.Migration;

namespace Pea.Core
{
    public delegate void NewEntitiesMergedToBestDelegate(IList<IEntity> bests);

    public delegate void LaunchTravelersDelegate(IList<IEntity> entityList, TravelerTypes travelerType);

    public interface IEngine
    {
        LaunchTravelersDelegate LaunchTravelers { get; set; }
        NewEntitiesMergedToBestDelegate NewEntityMergedToBest { get; set; }

        IRandom Random { get; set; }
        IAlgorithm Algorithm { get; set; }
        IProvider<IEntityCreator> EntityCreators { get; }
        IEntityCrossover EntityCrossover { get; }
        IEntityMutation EntityMutation { get; }
        IFitnessComparer FitnessComparer { get; }
        IProvider<IReplacement> Replacements { get; }
        IProvider<ISelection> Selections { get; }
        IReduction Reduction { get; set; }
        IEvaluation Evaluation { get; set; }
        ParameterSet Parameters { get; }

        void Init(IEvaluationInitData initData);
        StopDecision RunOnce();
        void MergeToBests(IList<IEntity> entities);
        void TravelersArrived(IList<IEntity> travelers);
    }
}