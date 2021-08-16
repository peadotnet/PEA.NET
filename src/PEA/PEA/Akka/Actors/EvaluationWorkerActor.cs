﻿using System;
using System.Collections.Generic;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;

namespace Pea.Akka.Actors
{
    public class EvaluationWorkerActor : ReceiveActor
    {
        private MultiKey IslandKey { get; }

        //private IEvaluation Calculator { get; }

        private string ActorPathName;

        private EvaluationBase Evaluation { get; }

        private ParameterSet Parameters { get; }

        private IEvaluationInitData EvaluationData { get; set; }

        public EvaluationWorkerActor(MultiKey islandKey, Type evaluatorType, ParameterSet parameters)
        {
            IslandKey = islandKey;

            ActorPathName = Self.Path.Name;
            //Calculator = (IEvaluation)Activator.CreateInstance(settings.Fitness);
            //Calculator.Init(initData);

            Evaluation = (EvaluationBase)TypeLoader.CreateInstance(evaluatorType, parameters);
            Parameters = parameters;

            Receive<InitEvaluator>(m => Init(m.InitData));
            Receive<IEntity>(e => Evaluate(e));
        }

        private void Init(IEvaluationInitData initData)
        {
            EvaluationData = initData;
            Evaluation.Init(initData);
        }

        private void Evaluate(IEntity entity)
        {
            var entityWithKey = new Dictionary<MultiKey, IEntity>();
            entityWithKey.Add(IslandKey, entity);

            var decodedEntity = Evaluation.Decode(IslandKey, entityWithKey);

            Sender.Tell(decodedEntity);
        }

        public static Props CreateProps(MultiKey islandKey, Type evaluatorType, ParameterSet parameters)
        {
            var props = Props.Create(() => new EvaluationWorkerActor(islandKey, evaluatorType, parameters));
            return props;
        }
    }
}
