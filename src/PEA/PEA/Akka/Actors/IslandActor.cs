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

        private IActorRef Starter { get; set; }

        public IslandActor(Configuration.Implementation.PeaSettings settings)
        {
            var actorPathName = Self.Path.ToString();
            Engine = IslandEngineFactory.Create(actorPathName, settings);
            IslandKey = CreateIslandKey(settings);
            Engine.Algorithm.SetEvaluationCallback(Evaluate);

            var evaluatorsCount = Engine.Parameters.GetInt(ParameterNames.IslandsCount);
            if (evaluatorsCount > 0)
            {
                Evaluator = Context.ActorOf(EvaluationSupervisorActor.CreateProps(IslandKey, settings.Evaluation, Engine.Parameters));
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
            Evaluator.Tell(m);
            Engine.Init(m.InitData);
            Engine.LaunchTravelers += LaunchTravelers;
            Self.Tell(Continue.Instance);
        }

        public void RunOneStep()
        {
            StopDecision stop = Engine.RunOnce();
            if (!stop.MustStop)
            {
                Self.Tell(Continue.Instance);
            }
            else
            {
                var result = new PeaResult(stop.Reasons, Engine.Algorithm.Population.Bests);
                Starter.Tell(result);
                Self.Tell(PoisonPill.Instance);
            }
        }

        public IList<IEntity> Evaluate(IList<IEntity> entityList)
        {
            if (entityList.Count == 0) return entityList;

            IList<IEntity> evaluatedEntities;

            //TODO: Evaluate entities locally
            //if (Evaluator == null)
            //{
            //    evaluatedEntities = new List<IEntity>();
            //    foreach (var entity in entityList)
            //    {
            //        var key = new MultiKey("TSP");
            //        var entityWithKey = new Dictionary<MultiKey, IEntity> {{key, entity}};
            //        var decodedEntity = Engine.Evaluation.Decode(key, entityWithKey);
            //        evaluatedEntities.Add(decodedEntity);
            //    }
            //}

            evaluatedEntities = Evaluator.Ask(entityList).GetAwaiter().GetResult() as IList<IEntity>;
            return evaluatedEntities;
        }

        public void LaunchTravelers(IList<IEntity> entityList, TravelerTypes travelerType)
        {
            foreach (var entity in entityList)
            {
                var traveler = new Travel(entityList, travelerType);
                Starter.Tell(traveler);
            }
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

        public static Props CreateProps(Configuration.Implementation.PeaSettings settings)
        {
            var props = Props.Create(() => new IslandActor(settings));
            return props;
        }
    }
}
