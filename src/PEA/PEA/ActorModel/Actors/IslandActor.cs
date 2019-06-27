using System;
using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;
using Pea.Island;

namespace Pea.ActorModel.Actors
{
    public class IslandActor : ReceiveActor
    {
        private IEngine Engine { get; }

        private IAlgorithm Algorithm { get; }

        private IActorRef Evaluator { get; set; }

        public IslandActor(PeaSettings settings)
        {
            Engine = IslandEngineFactory.Create(settings);

            var algorithmFactory = (IAlgorithmFactory)Activator.CreateInstance(settings.Algorithm);
            Algorithm = algorithmFactory.GetAlgorithm(Engine, Evaluate);
            Engine.Algorithm = Algorithm;

            Receive<InitEvaluator>(m => InitEvaluator(m));
            Receive<Continue>(m => RunOneStep());
        }

        private void InitEvaluator(InitEvaluator m)
        {
            Evaluator.Tell(m);
        }

        public void RunOneStep()
        {
            bool stop = Engine.RunOnce();
            if (!stop)
            {
                Self.Tell(Continue.Instance);
            }
        }

        public IList<IEntity> Evaluate(IList<IEntity> entityList)
        {
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
