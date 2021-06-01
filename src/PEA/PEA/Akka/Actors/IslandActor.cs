using System;
using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Akka.Messages;
using Pea.Core;
using Pea.Core.Island;
using Pea.Migration;
using ParameterNames = Pea.Core.Island.ParameterNames;

namespace Pea.Akka.Actors
{
    public class IslandActor : ReceiveActor
    {
        private MultiKey IslandKey { get; }

        private IEngine Engine { get; }

        private IActorRef Evaluator { get; }

        private IEvaluation LocalEvaluator { get; }

        private IActorRef Starter { get; set; }

        public IslandActor(Configuration.Implementation.PeaSettings settings, int seed)
        {
            var actorPathName = Self.Path.ToString();
            Engine = IslandEngineFactory.Create(actorPathName, settings, seed);
            IslandKey = CreateIslandKey(settings);
            Engine.Algorithm.SetEvaluationCallback(Evaluate);

            var evaluatorsCount = Engine.Parameters.GetInt(ParameterNames.EvaluatorsCount);
            if (evaluatorsCount > 4)
            {
                Evaluator = Context.ActorOf(EvaluationSupervisorActor.CreateProps(IslandKey, settings.Evaluation, Engine.Parameters));
            }
            else
			{
                LocalEvaluator = (IEvaluation)TypeLoader.CreateInstance(settings.Evaluation);
            }

            Receive<InitEvaluator>(m => InitEvaluator(m));
            Receive<Continue>(m => RunOneStep());
            Receive<Travel>(m => TravelersArrived(m));
            Receive<End>(m => End());
        }

        private static MultiKey CreateIslandKey(Configuration.Implementation.PeaSettings settings)
        {
            var key = new string[settings.SubProblemList.Count];
            for (int i = 0; i < settings.SubProblemList.Count; i++)
            {
                key[i] = settings.SubProblemList[i].Encoding.Key;
            }
            var islandKey = new MultiKey(key);
            return islandKey;
        }

        protected override void PreStart()
        {
            Debug.WriteLine($"Actor {Self.Path.Name} started!");
            Context.Parent.Tell(new CreatedSuccessfully());
            base.PreStart();
        }

        private void InitEvaluator(InitEvaluator m)
        {
            Starter = Sender;
            if (Evaluator != null)
            {
                Evaluator.Tell(m);
            }
            else
            {
                LocalEvaluator.Init(m.InitData);
            }

            Engine.Init(m.InitData);
            Engine.LaunchTravelers += LaunchTravelers;
            Self.Tell(Continue.Instance);
        }

        public void RunOneStep()
        {
            StopDecision stop = null;
            var algorithmCycleCount = Engine.Parameters.GetInt(Pea.Algorithm.ParameterNames.SubCycleRunCount) + 1;
            for (int i = 0; i < algorithmCycleCount; i++)
            {
                stop = Engine.RunOnce();
                if (stop.MustStop)
                {
                    var result = new PeaResult(stop.Reasons, Engine.Algorithm.Population.Bests);
                    Starter.Tell(result);
                    Self.Tell(PoisonPill.Instance);
                    break;
                }
            }

            Engine.Reduction.Reduct(Engine.Algorithm.Population.Entities);
            if (!stop.MustStop) Self.Tell(Continue.Instance);
        }

        public IList<IEntity> Evaluate(IList<IEntity> entityList)
        {
            if (entityList.Count == 0) return entityList;

            IList<IEntity> evaluatedEntities;

            //TODO: Evaluate entities locally

            if (Evaluator == null)
            {
                evaluatedEntities = new List<IEntity>(entityList.Count);
                for(int e = 0; e< entityList.Count; e++)
                {
                    var entityWithKey = new Dictionary<MultiKey, IEntity> { { IslandKey, entityList[e] } };
                    var decodedEntity = LocalEvaluator.Decode(IslandKey, entityWithKey);
                    evaluatedEntities.Add(decodedEntity);
                }
            }
            else
            {
                evaluatedEntities = Evaluator.Ask(entityList).GetAwaiter().GetResult() as IList<IEntity>;
            }
            return evaluatedEntities;
        }

        public void LaunchTravelers(IList<IEntity> entityList, TravelerTypes travelerType)
        {
            //for (int e = 0; e < entityList.Count;e++)
            //{
                var traveler = new Travel(entityList, travelerType);
                Starter.Tell(traveler);
            //}
        }

        private void TravelersArrived(Travel travel)
        {
            Engine.TravelersArrived(travel.Members);
        }

        private void End()
        {
            Self.Tell(PoisonPill.Instance);
        }

        protected override void PostStop()
        {
            Debug.WriteLine($"Actor {Self.Path.Name} stopped!");
            base.PostStop();
        }

        public static Props CreateProps(Configuration.Implementation.PeaSettings settings, int seed)
        {
            var props = Props.Create(() => new IslandActor(settings, seed));
            return props;
        }
    }
}
