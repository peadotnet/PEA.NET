using System;
using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using Pea.Configuration.Implementation;
using Pea.Core;

namespace Pea.Akka.Actors
{
    public class EvaluationWorkerActor : ReceiveActor
    {
        private MultiKey IslandKey { get; }

        private PeaSettings Settings { get; }

        //private IEvaluation Calculator { get; }

        private IEvaluation Evaluation { get; }

        private IEvaluationInitData EvaluationData { get; }

        public EvaluationWorkerActor(MultiKey islandKey, PeaSettings settings, IEvaluationInitData initData)
        {
            IslandKey = islandKey;
            Settings = settings;
            EvaluationData = initData;
            //Calculator = (IEvaluation)Activator.CreateInstance(settings.Fitness);
            //Calculator.Init(initData);

            //Evaluation = (IEvaluation)TypeLoader.CreateInstance(settings.Evaluation);
            Evaluation.Init(initData);

            Receive<IEntity>(e => Evaluate(e));
        }

        private void Evaluate(IEntity entity)
        {
            var entityWithKey = new Dictionary<MultiKey, IEntity>();
            entityWithKey.Add(IslandKey, entity);

            var decodedEntity = Evaluation.Decode(IslandKey, entityWithKey);

            var a = Self.Path.Name;

            Sender.Tell(decodedEntity);
        }

        public static Props CreateProps(MultiKey islandKey, PeaSettings settings, IEvaluationInitData initData)
        {
            var props = Props.Create(() => new EvaluationWorkerActor(islandKey, settings, initData));
            return props;
        }
    }
}
