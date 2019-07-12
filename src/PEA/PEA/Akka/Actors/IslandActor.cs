using System;
using System.Collections.Generic;
using System.Reflection;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Akka.Messages;
using Pea.Core;
using Pea.Core.Island;

namespace Pea.Akka.Actors
{
    public class IslandActor : ReceiveActor
    {
        private MultiKey IslandKey { get; }

        private IEngine Engine { get; }

        private IAlgorithm Algorithm { get; }

        private IActorRef Evaluator { get; set; }

        private IActorRef Starter { get; set; }

        public IslandActor(PeaSettings settings)
        {
            Engine = IslandEngineFactory.Create(settings);

            IslandKey = settings.Chromosomes[0].Keys; //TODO: multiple island topology

            var algorithmFactory = (IAlgorithmFactory)Activator.CreateInstance(settings.Algorithm);
            Algorithm = algorithmFactory.GetAlgorithm(Engine, Evaluate);
            Engine.Algorithm = Algorithm;

            Evaluator = Context.ActorOf(EvaluationSupervisorActor.CreateProps(IslandKey, settings));

            Receive<InitEvaluator>(m => InitEvaluator(m));
            Receive<Continue>(m => RunOneStep());
        }

        protected override void PreStart()
        {
            Context.Parent.Tell(new CreatedSuccessfully());
            base.PreStart();
        }

        private void InitEvaluator(InitEvaluator m)
        {
            Starter = Sender;
            Evaluator.Tell(m);
            Engine.Init();
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
                var result = new PeaResult(stop.Reasons, Algorithm.Population.Bests);
                Starter.Tell(result);
            }
        }

        public IList<IEntity> Evaluate(IList<IEntity> entityList)
        {
            //var result = new List<IEntity>();
            //foreach (var entity in entityList)
            //{
            //    var key = new MultiKey("TSP");
            //    var entityWithKey = new Dictionary<MultiKey, IEntity>();
            //    entityWithKey.Add(key, entity);

            //    var decodedEntity = Evaluation.Decode(key, entityWithKey);
            //    result.Add(decodedEntity);
            //}

            //return result;

            var evaluatedEntities = Evaluator.Ask(entityList).GetAwaiter().GetResult() as IList<IEntity>;
            return evaluatedEntities;
        }

        public static Props CreateProps(PeaSettings settings)
        {
            var props = Props.Create(() => new IslandActor(settings));
            return props;
        }
    }
}
