using System.Collections.Generic;
using Pea.Migration;

namespace Pea.Core
{
    public delegate void NewEntitiesMergedToBestDelegate(IList<IEntity> bests);

    public delegate void LaunchTravelersDelegate(IEntityList entityList, TravelerTypes travelerType);

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
        EvaluationBase Evaluation { get; set; }
        ParameterSet Parameters { get; }

        void Init(IEvaluationInitData initData);
        StopDecision RunOnce();
        void MergeToBests(IEntityList entities);
        void Reduct();
        void TravelersArrived(IEntityList travelers);
    }
}