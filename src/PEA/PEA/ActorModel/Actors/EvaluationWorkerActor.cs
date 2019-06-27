using System;
using Akka.Actor;
using Pea.ActorModel.Messages;
using Pea.Core;

namespace Pea.ActorModel.Actors
{
    public class EvaluationWorkerActor : ReceiveActor
    {
        private PeaSettings Settings { get; }

        private IEvaluation Calculator { get; }

        public EvaluationWorkerActor(PeaSettings settings, IEvaluationInitData initData)
        {
            Settings = settings;
            Calculator = (IEvaluation)Activator.CreateInstance(settings.Fitness);
            Calculator.Init(initData);
        }

        public static Props CreateProps(PeaSettings settings, IEvaluationInitData initData)
        {
            var props = Props.Create(() => new EvaluationWorkerActor(settings, initData));
            return props;
        }
    }
}
